using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Web;
using System.Web.Compilation;

namespace eSchoolWeb
{
    public sealed class MyResourceProviderFactory : ResourceProviderFactory
    {
        public override IResourceProvider CreateGlobalResourceProvider(string className)
        {
            return new MyResourceProvider(null, className);
        }

        public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
        {
            virtualPath = Path.GetFileName(virtualPath);
            return new MyResourceProvider(virtualPath, null);
        }
    }

    public sealed class MyResourceProvider : IResourceProvider
    {
        private string _VirtualPath;
        private string _ClassName;
        private IDictionary _ResourceCache;
        private MyResourceReader _ResourceReader = null;

        public MyResourceProvider(string virtualPath, string className)
        {
            _VirtualPath = virtualPath;
            _ClassName = className;
        }

        private IDictionary GetResourceCache(string cultureName)
        {
            cultureName = cultureName == null ? String.Empty : cultureName.Trim();
            if (cultureName.Equals("zh-tw", StringComparison.CurrentCultureIgnoreCase))
            {
                cultureName = String.Empty;
            }

            #region 取得企業類別
            string corpAttrID = String.Empty;

            #region [MDY:20210401] 原碼修正 (土銀多語系沒有區分企業類別)
            //{
            //    HttpCookie cookie = HttpContext.Current.Request.Cookies["CorpAttrID"];
            //    if (cookie != null)
            //    {
            //        cookie.HttpOnly = true;
            //        corpAttrID = cookie.Value == null ? String.Empty : cookie.Value.Trim();
            //    }
            //}
            #endregion
            #endregion

            #region 產生 fileKey
            string fileKey = null;
            string fileKey1 = null;	//完整
            string fileKey2 = null;	//含語系
            string fileKey3 = null;	//含企業類別
            string fileKey4 = _ClassName;	//僅 class name
            if (corpAttrID.Length > 0 && cultureName.Length > 0)
            {
                fileKey1 = String.Format("{0}_{1}.{2}", _ClassName, corpAttrID, cultureName);
                fileKey2 = String.Format("{0}.{1}", _ClassName, cultureName);
                fileKey3 = String.Format("{0}_{1}", _ClassName, corpAttrID);
                fileKey = fileKey1;
            }
            else if (cultureName.Length > 0)
            {
                fileKey1 = String.Empty;
                fileKey2 = String.Format("{0}.{1}", _ClassName, cultureName);
                fileKey3 = String.Empty;
                fileKey = fileKey2;
            }
            else if (corpAttrID.Length > 0)
            {
                fileKey1 = String.Empty;
                fileKey2 = String.Empty;
                fileKey3 = String.Format("{0}_{1}", _ClassName, corpAttrID);
                fileKey = fileKey3;
            }
            else
            {
                fileKey1 = String.Empty;
                fileKey2 = String.Empty;
                fileKey3 = String.Empty;
                fileKey = fileKey4;
            }
            #endregion

            if (_ResourceCache == null)
            {
                _ResourceCache = new ListDictionary();
            }

            IDictionary dicResource = _ResourceCache[fileKey] as IDictionary;
            if (dicResource == null)
            {
                ListDictionary resources = new ListDictionary();
                try
                {
                    string resxFile = HttpContext.Current.Server.MapPath(String.Format("~/App_GlobalResources/{0}.resx", fileKey));
                    if (!File.Exists(resxFile) && fileKey != fileKey4)
                    {
                        bool noResx = true;
                        if (noResx && fileKey1.Length > 0 && fileKey != fileKey1)
                        {
                            resxFile = HttpContext.Current.Server.MapPath(String.Format("~/App_GlobalResources/{0}.resx", fileKey1));
                            if (File.Exists(resxFile))
                            {
                                fileKey = fileKey1;
                                noResx = false;
                            }
                        }
                        if (noResx && fileKey2.Length > 0 && fileKey != fileKey2)
                        {
                            resxFile = HttpContext.Current.Server.MapPath(String.Format("~/App_GlobalResources/{0}.resx", fileKey2));
                            if (File.Exists(resxFile))
                            {
                                fileKey = fileKey2;
                                noResx = false;
                            }
                        }
                        if (noResx && fileKey3.Length > 0 && fileKey != fileKey3)
                        {
                            resxFile = HttpContext.Current.Server.MapPath(String.Format("~/App_GlobalResources/{0}.resx", fileKey3));
                            if (File.Exists(resxFile))
                            {
                                fileKey = fileKey3;
                                noResx = false;
                            }
                        }
                        if (noResx)
                        {
                            resxFile = HttpContext.Current.Server.MapPath(String.Format("~/App_GlobalResources/{0}.resx", fileKey4));
                            fileKey = fileKey4;
                        }

                        dicResource = _ResourceCache[fileKey] as IDictionary;
                    }
                    if (dicResource == null && resxFile.Length > 0 && File.Exists(resxFile))
                    {
                        ResXResourceReader rsxr = new ResXResourceReader(resxFile);
                        IDictionaryEnumerator id = rsxr.GetEnumerator();
                        foreach (DictionaryEntry d in rsxr)
                        {
                            string key = d.Key.ToString();
                            string val = d.Value.ToString();
                            resources.Add(key, val);
                        }
                        _ResourceCache[fileKey] = resources;
                    }
                }
                catch
                {
                }
                if (dicResource == null)
                {
                    dicResource = resources;
                }
            }

            return dicResource;
        }

        object IResourceProvider.GetObject(string resourceKey, CultureInfo culture)
        {
            string cultureName = null;
            if (culture != null)
            {
                cultureName = culture.Name;
            }
            else
            {
                cultureName = CultureInfo.CurrentUICulture.Name;
            }

            object value = this.GetResourceObject(resourceKey, cultureName);
            return value;
        }

        Object GetResourceObject(string resourceKey, string cultureName)
        {
            object value = this.GetResourceCache(cultureName)[resourceKey];
            if (value == null)
            {
                value = resourceKey;
            }
            return value;
        }

        IResourceReader IResourceProvider.ResourceReader
        {
            get
            {
                if (_ResourceReader == null)
                {
                    _ResourceReader = new MyResourceReader(GetResourceCache(null));
                }
                return _ResourceReader as IResourceReader;
            }
        }

        #region [TMP] 暫時用不到
        //#region IImplicitResourceProvider Members

        ///// <summary>
        ///// Called when an ASP.NET Page is compiled asking for a collection
        ///// of keys that match a given control name (keyPrefix). This
        ///// routine for example returns control.Text,control.ToolTip from the
        ///// Resource collection if they exist when a request for "control"
        ///// is made as the key prefix.
        ///// </summary>
        ///// <param name="keyPrefix"></param>
        ///// <returns></returns>
        //public ICollection GetImplicitResourceKeys(string keyPrefix)
        //{
        //    List<ImplicitResourceKey> keys = new List<ImplicitResourceKey>();

        //    IDictionaryEnumerator Enumerator = this.ResourceReader.GetEnumerator();
        //    if (Enumerator == null)
        //        return keys; // Cannot return null!

        //    foreach (DictionaryEntry dictentry in this.ResourceReader)
        //    {
        //        string key = (string) dictentry.Key;

        //        if (key.StartsWith(keyPrefix + ".", StringComparison.InvariantCultureIgnoreCase) == true)
        //        {
        //            string keyproperty = String.Empty;
        //            if (key.Length > (keyPrefix.Length + 1))
        //            {
        //                int pos = key.IndexOf('.');
        //                if ((pos > 0) && (pos == keyPrefix.Length))
        //                {
        //                    keyproperty = key.Substring(pos + 1);
        //                    if (String.IsNullOrEmpty(keyproperty) == false)
        //                    {
        //                        //Debug.WriteLine("Adding Implicit Key: " + keyPrefix + " - " + keyproperty);
        //                        ImplicitResourceKey implicitkey = new ImplicitResourceKey(String.Empty, keyPrefix, keyproperty);
        //                        keys.Add(implicitkey);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return keys;
        //}


        //public object GetObject(ImplicitResourceKey implicitKey, CultureInfo culture)
        //{
        //    string resourceKey = ConstructFullKey(implicitKey);

        //    string cultureName = null;
        //    if (culture != null)
        //    {
        //        cultureName = culture.Name;
        //    }
        //    else
        //    {
        //        cultureName = CultureInfo.CurrentUICulture.Name;
        //    }

        //    return this.GetResourceObject(resourceKey, cultureName);
        //}

        //private static string ConstructFullKey(ImplicitResourceKey entry)
        //{
        //    string text = entry.KeyPrefix + "." + entry.Property;
        //    if (entry.Filter.Length > 0)
        //    {
        //        text = entry.Filter + ":" + text;
        //    }
        //    return text;
        //}
        #endregion
    }

    public sealed class MyResourceReader : IResourceReader
    {
        private IDictionary _resources;

        public MyResourceReader(IDictionary resources)
        {
            _resources = resources;
        }
        IDictionaryEnumerator IResourceReader.GetEnumerator()
        {
            return _resources.GetEnumerator();
        }
        void IResourceReader.Close()
        {
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _resources.GetEnumerator();
        }
        void IDisposable.Dispose()
        {
        }
    }
}