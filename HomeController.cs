
public class HomeController:Controller
{
  [HttpGet]
  public IActionResult LoginPage()
    {
      return View();
    }
    
    [HttpPost]
  public IActionResult Signin(string userid,string password)
  {
      bool status = false;
      string message;
      if(userid != "prakhar@gmail.com" && password != "123456")
      {
      message = "Invalid userid and password";
      }
      else
      {
      bool loginuser = CallLoginApi(userid,password);
      return RedirectToAction("Uploadpdf");
      status = true;
      } 
      return Json(new {status = success,message = message});
  }
  
  [HttpGet]
  public IActionResult Uploadpdf()
  {
    return View();
  }
  [HttpPost]
  public IActionResult UploadpdfApi()
  {
  bool status=false;
  string message ="something wents wrong";
  string filePath = "https://ctapi.kilobytetech.com/api/folder/5e2c5b5d5323c70ae924a815/addPdf";
  using (var httpClient = new HttpClient())
  {
    using (var form = new MultipartFormDataContent())
    {
      using (var fs = File.OpenRead(filePath))
      {
        using (var streamContent = new StreamContent(fs))
         {
           using (var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync()))
             {
               fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
               form.Add(fileContent, "file", Path.GetFileName(filePath));
               HttpResponseMessage response = await httpClient.PostAsync(url, form);
               status = true;
               message ="File added Successfully";
              }
          }
        }
      }
    }
    return Json(new{success=status,message=message});
  }
  [NonAction]
  public Task<bool> CallLoginApi(string loginId,string loginpassword)
  {
  HttpResponse response;
  Uri url= new uri("https://ctapi.kilobytetech.com/api/user/login");
  HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
  request.Method = "POST";
  request.Accept = "application/json";
  using (var streamWriter = new StreamWriter(request.GetRequestStream()))
    {
      var modal = new
      {
        EmailId = loginId,
        Password = loginpassword
      };
      string json = JsonConvert.SerializeObject(modal);
      streamWriter.Write(json);
      streamWriter.Flush();
      streamWriter.Close();
      using (response = request.GetResponse() as HttpWebResponse)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string results = reader.ReadToEnd();
            ApiLoginResult loginStatus = JsonConvert.DeserializeObject<ApiLoginResult>(results);
            foreach (ApiLoginResult login in loginStatus.value)
                {
                  if (loginStatus.Status == 0)
                      {
                         result = true;
                      }
                }
         }
         return result;
    }
  }
}
