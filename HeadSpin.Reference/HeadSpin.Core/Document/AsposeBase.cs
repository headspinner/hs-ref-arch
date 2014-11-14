using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadSpin.Core.Utilities.Document
{
    internal class AsposeBase
    {
        protected string BucketName { get; set; }
        protected string AsposePreConfigStorageName { get; set; }
        protected string Environment { get; set; }

        public AsposeBase()
        {
            Aspose.Cloud.Common.AsposeApp.AppKey = ConfigHelper.GetAppSettingValue("aspose-appkey");
            Aspose.Cloud.Common.AsposeApp.AppSID = ConfigHelper.GetAppSettingValue("aspose-appsid");
            Aspose.Cloud.Common.Product.BaseProductUri = "http://api.aspose.com/v1.1";

            this.Environment = ConfigHelper.GetAppSettingValue("EnvironmentName").ToLower();
            this.BucketName = ConfigHelper.GetAppSettingValue("amazon-s3-bucket-name");
            this.AsposePreConfigStorageName = ConfigHelper.GetAppSettingValue("aspose-preconfigured-storage-name");
        }
         
    }
}
