namespace ManualTools.Common;

public class CommonResponse<T>
{


  public T? Data { get; set; }
  public string Error { get; set; }

  public CommonResponse(string error)
  {
    Error = error;
  }

  public CommonResponse(T data)
  {
    Error = string.Empty;
    Data = data;
  }
}