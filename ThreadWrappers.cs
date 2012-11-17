using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CJ {
    #region Callback Delegates
    public delegate void ThreadProc();
    public delegate void ThreadProc<ParameterType>(ParameterType arg);
    public delegate void ThreadProc<Parameter1Type, Parameter2Type>(Parameter1Type arg1, Parameter2Type arg2);
    public delegate ReturnType ThreadMethod<ReturnType>();
    public delegate ReturnType ThreadMethod<ReturnType, ParameterType>(ParameterType arg);
    public delegate ReturnType ThreadMethod<ReturnType, Parameter1Type, Parameter2Type>(Parameter1Type arg1, Parameter2Type arg2);
    #endregion
    #region event arguments class CancelEventArgs
    public class CancelEventArgs : EventArgs {
        #region Private Fields
        private bool _cancel;
        #endregion
        #region Public Properties
        public bool Cancel { get { return _cancel; } set { _cancel = value; } }
        #endregion
        #region Constructor
        public CancelEventArgs() { _cancel = false; }
        #endregion
    }
    #endregion
    #region abstract class ThreadWrapperBase
    public abstract class ThreadWrapperBase {
        #region Protected Fields
        protected Thread _thread;
        protected bool _started;
        #endregion
        #region Abstract Methods
        /// <summary>
        /// This method should be overriden in child classes and the implementation will
        /// simply be to call the actual thread parametrized method and save its return value
        /// in the result field (which should be added if needed)
        /// </summary>
        public abstract void Run();
        #endregion
        #region Implementation Methods
        public void Start() {
            CancelEventArgs e = new CancelEventArgs(); OnThreadStarting(e);
            if (!e.Cancel) { _thread.Start(); OnThreadStarted(EventArgs.Empty); _started = true; }
        }
        public void Abort() {
            CancelEventArgs e = new CancelEventArgs(); OnThreadAborting(e);
            if (!e.Cancel) { _thread.Abort(); OnThreadAborted(EventArgs.Empty); }
        }
        public void Join() { _thread.Join(); }
        public void Join(int millisecondsTimeOut) { _thread.Join(millisecondsTimeOut); }
        public void Join(TimeSpan timeout) { _thread.Join(timeout); }
        public bool Started { get { return _started; } }
        #endregion
        #region Events
        public event EventHandler<CancelEventArgs> ThreadStarting;
        public event EventHandler<CancelEventArgs> ThreadAborting;

        public event EventHandler ThreadStarted;
        public event EventHandler ThreadAborted;
        #endregion
        #region Event Firing Methods
        protected virtual void OnThreadStarting(CancelEventArgs args) { if (ThreadStarting != null) { ThreadStarting(this, args); } }
        protected virtual void OnThreadAborting(CancelEventArgs args) { if (ThreadAborting != null) { ThreadAborting(this, args); } }

        protected virtual void OnThreadStarted(EventArgs args) { if (ThreadStarted != null) { ThreadStarted(this, args); } }
        protected virtual void OnThreadAborted(EventArgs args) { if (ThreadAborted != null) { ThreadAborted(this, args); } }
        #endregion
        #region Protected Constructor
        protected ThreadWrapperBase() { _thread = new Thread(new ThreadStart(Run)); _started = false; }
        #endregion
    }
    #endregion
    #region abstract class MethodThreadWrapperBase<ReturnType>
    public abstract class MethodThreadWrapperBase<ReturnType> : ThreadWrapperBase {
        #region Protected Fields
        protected ReturnType _return;
        protected bool _finished;
        #endregion
        #region Properties
        public ReturnType Result { get { return _return; } }
        public bool ResultReady { get { return _finished; } }
        #endregion
        #region Events
        public event EventHandler Finished;
        #endregion
        #region Event Firing Methods
        protected virtual void OnFinished(EventArgs args) { if (Finished != null) { Finished(this, args); } }
        #endregion
        #region Constructor
        protected MethodThreadWrapperBase() : base() { }
        #endregion
    }
    #endregion
    //Procedure (methods that return void) Thread Wrappers
    #region class ProcThreadWrapper - void(void) wrapper
    public class ProcThreadWrapper : ThreadWrapperBase {
        #region Protected Fields
        protected ThreadProc _method;
        #endregion
        #region Base Overrides
        public override void Run() {
            _method();
        }
        #endregion
        #region Constructor(s)
        protected ProcThreadWrapper() : base() { }
        public ProcThreadWrapper(ThreadProc method)
            : base() {
            _method = method;
        }
        #endregion
    }
    #endregion
    #region class ProcThreadWrapper<ParameterType> - void(ParameterType) wrappers
    public class ProcThreadWrapper<ParameterType> : ThreadWrapperBase {
        #region Protected Fields
        protected ParameterType _parameter;
        protected ThreadProc<ParameterType> _method;
        #endregion
        #region Base Overrides
        public override void Run() {
            _method(_parameter);
        }
        #endregion
        #region Constructor(s)
        protected ProcThreadWrapper() : base() { }
        public ProcThreadWrapper(ThreadProc<ParameterType> method, ParameterType parameter)
            : base() {
            _parameter = parameter; _method = method;
        }
        #endregion
    }
    #endregion
    #region class ProcThreadWrapper<Parameter1Type, Parameter2Type> - void(Parameter1Type, Parameter2Type) wrappers
    public class ProcThreadWrapper<Parameter1Type, Parameter2Type> : ThreadWrapperBase {
        #region Protected Fields
        protected Parameter1Type _parameter1;
        protected Parameter2Type _parameter2;
        protected ThreadProc<Parameter1Type, Parameter2Type> _method;
        #endregion
        #region Base Overrides
        public override void Run() {
            _method(_parameter1, _parameter2);
        }
        #endregion
        #region Constructor(s)
        protected ProcThreadWrapper() : base() { }
        public ProcThreadWrapper(ThreadProc<Parameter1Type, Parameter2Type> method, Parameter1Type parameter1, Parameter2Type parameter2)
            : base() {
            _parameter2 = parameter2; _parameter1 = parameter1; _method = method;
        }
        #endregion
    }
    #endregion
    //Method Thread Wrappers
    #region class MethodThreadWrapper<ReturnType> - ReturnType(void) wrappers
    public class MethodThreadWrapper<ReturnType> : MethodThreadWrapperBase<ReturnType> {
        #region Protected Fields
        protected ThreadMethod<ReturnType> _method;
        #endregion
        #region Base Overrides
        public override void Run() {
            _return = _method();
            _finished = true;
            OnFinished(EventArgs.Empty);
        }
        #endregion
        #region Constructor(s)
        protected MethodThreadWrapper() : base() { _finished = false; }
        public MethodThreadWrapper(ThreadMethod<ReturnType> method)
            : base() {
            _method = method; _finished = false;
        }
        #endregion
    }
    #endregion
    #region class MethodThreadWrapper<ReturnType, ParameterType> - ReturnType(ParameterType) wrappers
    public class MethodThreadWrapper<ReturnType, ParameterType> : MethodThreadWrapperBase<ReturnType> {
        #region Protected Fields
        protected ThreadMethod<ReturnType, ParameterType> _method;
        protected ParameterType _parameter;
        #endregion
        #region Base Overrides
        public override void Run() {
            _return = _method(_parameter);
            _finished = true;
            OnFinished(EventArgs.Empty);
        }
        #endregion
        #region Constructor(s)
        protected MethodThreadWrapper() : base() { _finished = false; }
        public MethodThreadWrapper(ThreadMethod<ReturnType, ParameterType> method, ParameterType parameter)
            : base() {
            _method = method; _finished = false; _parameter = parameter;
        }
        #endregion
    }
    #endregion
    #region class MethodThreadWrapper<ReturnType, Parameter1Type, Parameter2Type> - ReturnType(Parameter1Type, Parameter2Type) wrappers
    public class MethodThreadWrapper<ReturnType, Parameter1Type, Parameter2Type> : MethodThreadWrapperBase<ReturnType> {
        #region Protected Fields
        protected ThreadMethod<ReturnType, Parameter1Type, Parameter2Type> _method;
        protected Parameter1Type _parameter1;
        protected Parameter2Type _parameter2;
        #endregion
        #region Base Overrides
        public override void Run() {
            _return = _method(_parameter1, _parameter2);
            _finished = true;
            OnFinished(EventArgs.Empty);
        }
        #endregion
        #region Constructor(s)
        protected MethodThreadWrapper() : base() { _finished = false; }
        public MethodThreadWrapper(ThreadMethod<ReturnType, Parameter1Type, Parameter2Type> method, Parameter1Type parameter1, Parameter2Type parameter2)
            : base() {
            _method = method; _finished = false; _parameter1 = parameter1; _parameter2 = parameter2;
        }
        #endregion
    }
    #endregion
}
