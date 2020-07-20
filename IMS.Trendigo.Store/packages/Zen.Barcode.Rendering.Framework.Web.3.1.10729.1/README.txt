Barcode Rendering Framework Release.3.1.10729 components for Asp.Net

From http://barcoderender.codeplex.com/ 30th June 2014

LICENSE
http://barcoderender.codeplex.com/license


Usage
------
Download the source and look at the sample applications from http://barcoderender.codeplex.com/releases/

Zen.Barcode.Web.dll contains an Http handler and a Route handler which will return barcode image files

Zen.Barcode.Design.dll contains a Control Designer for design time.

For Mvc: reference Zen.Barcode.Web.dll and optionally lib\Zen.Barcode.Mvc.dll. You can then:
  1) Add a barcode image route to your MVC application:
  routes.Add("BarcodeImaging", new Route("Barcode/{id}",new BarcodeImageRouteHandler()));
  2) Use it with &lt;img src="@Url.Barcode("123456", BarcodeSymbology.Code128, 30, 1, true)" /&gt;
  The HTML helper method will create a URL to return your barcode.

Windows Test Application
----------------
Run tools\BarcodeRender.exe. It expects to find Zen.Barcode.Core.dll in the same directory.