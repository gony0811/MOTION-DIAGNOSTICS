using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace MotionDiagnostics.Navigation
{
    public class NavigationServiceEx
    {
        public event NavigatedEventHandler Navigated;

        public event NavigationFailedEventHandler NavigationFailed;

        // 캐시된 페이지를 저장하는 Dictionary
        private readonly Dictionary<Uri, object> _pageCache = new Dictionary<Uri, object>();

        private Frame _frame;

        public Frame Frame
        {
            get
            {
                if (this._frame == null)
                {
                    this._frame = new Frame()
                    {
                        NavigationUIVisibility = NavigationUIVisibility.Hidden,
                        CacheMode = new BitmapCache(),

                    };
                    this.RegisterFrameEvents();
                }

                return this._frame;
            }
            set
            {
                this.UnregisterFrameEvents();
                this._frame = value;
                this.RegisterFrameEvents();
            }
        }

        public bool CanGoBack => this.Frame.CanGoBack;

        public bool CanGoForward => this.Frame.CanGoForward;

        public void GoBack() => this.Frame.GoBack();

        public void GoForward() => this.Frame.GoForward();


        /// <summary>
        /// Navigate to the specified page.
        /// 처음 호출되는 Page는 캐시에 저장하고 이후 호출 시 캐시된 Page를 사용한다.
        /// </summary>
        /// <param name="sourcePageUri"></param>
        /// <param name="extraData"></param>
        /// <returns></returns>
        public bool Navigate(Uri sourcePageUri, object extraData = null)
        {
            if (this.Frame.CurrentSource != sourcePageUri)
            {
                if (_pageCache.TryGetValue(sourcePageUri, out var cachedPage))
                {
                    return this.Frame.Navigate(cachedPage, extraData);
                }
                else
                {
                    var result = this.Frame.Navigate(sourcePageUri, extraData);
                    if (result)
                    {
                        this.Frame.Navigated += (s, e) =>
                        {
                            if (e.Uri == sourcePageUri && !_pageCache.ContainsKey(sourcePageUri))
                            {
                                _pageCache[sourcePageUri] = (Page)this.Frame.Content;
                            }
                        };
                    }
                    return result;
                }
            }

            return false;
        }

        public bool Navigate(Type sourceType)
        {
            if (this.Frame.NavigationService?.Content?.GetType() != sourceType)
            {
                var instance = this.Frame.Navigate(Activator.CreateInstance(sourceType));
                return true;
            }

            return false;
        }

        private void RegisterFrameEvents()
        {
            if (this._frame != null)
            {
                this._frame.Navigated += this.Frame_Navigated;
                this._frame.NavigationFailed += this.Frame_NavigationFailed;
            }
        }

        private void UnregisterFrameEvents()
        {
            if (this._frame != null)
            {
                this._frame.Navigated -= this.Frame_Navigated;
                this._frame.NavigationFailed -= this.Frame_NavigationFailed;
            }
        }

        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e) => this.NavigationFailed?.Invoke(sender, e);

        private void Frame_Navigated(object sender, NavigationEventArgs e) => this.Navigated?.Invoke(sender, e);
    }
}