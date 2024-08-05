namespace View
{
    public interface IView
    {
        void Init();
        void AfterInit();
        void AfterShow();
        void AfterHide();
        void AfterClose();
        void Release();
    }
}