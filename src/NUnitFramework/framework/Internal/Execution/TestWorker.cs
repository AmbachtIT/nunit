// Copyright (c) Charlie Poole, Rob Prouse and Contributors. MIT License - see LICENSE.txt

using System;
using System.Threading;
using NUnit.Framework.Interfaces;

namespace NUnit.Framework.Internal.Execution
{
    /// <summary>
    /// A TestWorker pulls work items from a queue
    /// and executes them.
    /// </summary>
    public class TestWorker
    {
        private static readonly Logger log = InternalTrace.GetLogger("TestWorker");

        private Thread _workerThread;

        private int _workItemCount = 0;

        private bool _running;

        #region Events

        /// <summary>
        /// Event handler for TestWorker events
        /// </summary>
        /// <param name="worker">The TestWorker sending the event</param>
        /// <param name="work">The WorkItem that caused the event</param>
        public delegate void TestWorkerEventHandler(TestWorker worker, WorkItem work);

        /// <summary>
        /// Event signaled immediately before executing a WorkItem
        /// </summary>
        public event TestWorkerEventHandler Busy;

        /// <summary>
        /// Event signaled immediately after executing a WorkItem
        /// </summary>
        public event TestWorkerEventHandler Idle;

        #endregion

        #region Constructor

        /// <summary>
        /// Construct a new TestWorker.
        /// </summary>
        /// <param name="queue">The queue from which to pull work items</param>
        /// <param name="name">The name of this worker</param>
        public TestWorker(WorkItemQueue queue, string name)
        {
            Guard.ArgumentNotNull(queue, nameof(queue));

            WorkQueue = queue;
            Name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The WorkItemQueue from which this worker pulls WorkItems
        /// </summary>
        public WorkItemQueue WorkQueue { get; }

        /// <summary>
        /// The name of this worker - also used for the thread
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Indicates whether the worker thread is running
        /// </summary>
        public bool IsAlive => _workerThread.IsAlive;

        #endregion

        /// <summary>
        /// Our ThreadProc, which pulls and runs tests in a loop
        /// </summary>
        private WorkItem _currentWorkItem;

        private void TestWorkerThreadProc()
        {
            _running = true;

            try
            {
                while (_running)
                {
                    _currentWorkItem = WorkQueue.Dequeue();
                    if (_currentWorkItem == null)
                        break;

                    log.Info("{0} executing {1}", _workerThread.Name, _currentWorkItem.Name);

                    _currentWorkItem.TestWorker = this;

                    // During this Busy call, the queue state may be saved.
                    // This gives us a new set of queues, which are initially
                    // empty. The intention is that only children of the current
                    // executing item should make use of the new set of queues.
                    // TODO: If we had a separate NonParallelTestWorker, it
                    // could simply create the isolated queue without any
                    // worrying about competing workers.
                    Busy(this, _currentWorkItem);

                    // Because we execute the current item AFTER the queue state
                    // is saved, its children end up in the new queue set.
                    _currentWorkItem.Execute();

                    // This call may result in the queues being restored. There
                    // is a potential race condition here. We should not restore
                    // the queues unless all child items have finished.
                    Idle(this, _currentWorkItem);

                    ++_workItemCount;
                }
            }
            finally
            {
                log.Info("{0} stopping - {1} WorkItems processed.", Name, _workItemCount);
            }
        }

        /// <summary>
        /// Create thread and start processing work items.
        /// </summary>
        public void Start()
        {
            _workerThread = new Thread(new ThreadStart(TestWorkerThreadProc));
            _workerThread.Name = Name;

            try
            {
                _workerThread.SetApartmentState(WorkQueue.TargetApartment);
            }
            catch (PlatformNotSupportedException)
            {
            }

            log.Info("{0} starting on thread [{1}]", Name, _workerThread.ManagedThreadId);
            _workerThread.Start();
        }

        private readonly object cancelLock = new object();

        /// <summary>
        /// Stop the thread, either immediately or after finishing the current WorkItem
        /// </summary>
        /// <param name="force">true if the thread should be aborted, false if it should allow the currently running test to complete</param>
        public void Cancel(bool force)
        {
            if (force)
                _running = false;

            lock (cancelLock)
                if (_workerThread != null && _currentWorkItem != null)
                {
                    _currentWorkItem.Cancel(force);
                    if (force)
                        _currentWorkItem = null;
                }
        }
    }
}
