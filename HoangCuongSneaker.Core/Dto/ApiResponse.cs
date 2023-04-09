using System.Collections;
using System.Linq;

namespace HoangCuongSneaker.Core.Dto
{
    public class ApiResponse
    {

        public DateTime Time { get; set; }
        public List<object> Data { get; set; }
        public object TraceId { get; set; }
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public ApiResponse()
        {
            Time = DateTime.Now;
            Data = new List<object>();
            TraceId = string.Empty;
            IsSuccessful = true;
            ErrorMessage = string.Empty;
        }

        /// <summary>
        /// trả về respone với message lỗi
        /// </summary>
        /// <param name="exception"></param>
        public void OnFailure(Exception exception)
        {
            IsSuccessful = false;
            Time = DateTime.Now;
            TraceId = string.Empty;
            ErrorMessage = exception.Message;
        }

        /// <summary>
        /// trả về Data dạng list
        /// </summary>
        /// <param name="data"></param>
        public void OnSuccess(object? data)
        {
            Time = DateTime.Now;
            if (data is not null && data is IList)
            {
                Data = ((IEnumerable)data).Cast<object>().ToList();
            }
            else if(data is not null)
            {
                Data.Add(data);
            }
            TraceId = string.Empty;
            IsSuccessful = true;
            ErrorMessage = string.Empty;
        }
    }
}