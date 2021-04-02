
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