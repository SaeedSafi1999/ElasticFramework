namespace ElasticFramework.DTOs
{
    /// <summary>
    /// response with data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ElasticResponse<T>
    {
        public T? Data { get; set; }
        public bool IsSucces { get; set; }
        public string Message { get; set; }
        public Exception? Exception { get; set; }



        /// <summary>
        /// return data if every thing works fine
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static ElasticResponse<T> Ok(T Data)
            => new ElasticResponse<T>
            {
                Data = Data,
                IsSucces = true,
                Message = "Operation Succeed"
            };


        /// <summary>
        /// return failed result with exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static ElasticResponse<T> Fail(Exception ex)
            => new ElasticResponse<T>
            {
                IsSucces = false,
                Message = $"Operation failed ERROR:{ex.Message}",
                Exception = ex
            };

        /// <summary>
        /// return failed result with custom message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static ElasticResponse<T> Fail(string message)
            => new ElasticResponse<T>
            {
                IsSucces = false,
                Message = $"Operation failed Message:{message}",
            };
    }

    /// <summary>
    /// normal response
    /// </summary>
    public class ElasticResponse
    {
        public bool IsSucces { get; set; }
        public string Message { get; set; }
        public Exception? Exception { get; set; }



        /// <summary>
        /// return data if every thing works fine
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static ElasticResponse Ok()
            => new ElasticResponse
            {
                IsSucces = true,
                Message = "Operation Succeed"
            };


        /// <summary>
        /// return failed result with exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static ElasticResponse Fail(Exception ex)
            => new ElasticResponse
            {
                IsSucces = false,
                Message = $"Operation failed ERROR:{ex.Message}",
                Exception = ex
            };



        /// <summary>
        /// return failed result with custom message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static ElasticResponse Fail(string message)
            => new ElasticResponse
            {
                IsSucces = false,
                Message = $"Operation failed Message:{message}",
            };

    }
}
