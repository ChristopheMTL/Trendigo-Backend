using System;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace IMS.Utilities.PaymentAPI.Client
{
    public class ApiAuthentication
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public X509Certificate2 GetCertificateByThumbprint(string certificateThumbPrint, StoreLocation certificateStoreLocation)
        {
            X509Certificate2 certificate = null;

            X509Store certificateStore = new X509Store(certificateStoreLocation);
            certificateStore.Open(OpenFlags.ReadOnly);


            X509Certificate2Collection certCollection = certificateStore.Certificates;

            foreach (X509Certificate2 cert in certCollection)
            {
                if (cert.Thumbprint != null && cert.Thumbprint.Equals(certificateThumbPrint, StringComparison.OrdinalIgnoreCase))
                {
                    certificate = cert;
                    break;
                }
            }

            if (certificate == null)
            {
                logger.ErrorFormat(CultureInfo.InvariantCulture, "Certificate with thumbprint {0} not found", certificateThumbPrint);
            }

            return certificate;
        }

        public X509Certificate GetCertificateByFileLocation(string certificateLocation, string password)
        {
            X509Certificate certificate = null;

            certificate = new X509Certificate(certificateLocation, password);

            if (certificate == null)
            {
                logger.ErrorFormat(CultureInfo.InvariantCulture, "Certificate with Filename {0} not found", certificateLocation);
            }

            return certificate;
        }
    }
}
