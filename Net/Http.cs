using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProtoSim.DotNetUtilities.Net {
    public class Http : IDisposable {
        private bool disposedValue;
        private static readonly HttpClient _client = new() { BaseAddress = null, Timeout = TimeSpan.FromSeconds(10) };

        public static bool SetBaseAddress(Uri? uri) {
            if (uri == null)
                return false;

            _client.BaseAddress = uri;
            return true;
        }

        public static bool SetBaseAddress(string? uriString) {
            if (string.IsNullOrEmpty(uriString))
                return false;

            _client.BaseAddress = new Uri(uriString);
            return true;
        }

        public static void EnableCache() => _client.DefaultRequestHeaders.CacheControl = new() { NoCache = false };

        public static void DisableCache() => _client.DefaultRequestHeaders.CacheControl = new() { NoCache = true };

        public static void SetTimeout(TimeSpan timeSpan) {
            _client.Timeout = timeSpan;
        }

        public static bool SetTimeout(int seconds) {
            if (seconds < 0)
                return false;

            _client.Timeout = TimeSpan.FromSeconds(seconds);
            return true;
        }

        public async static Task<HttpResponseMessage?> SendRequestAsync(HttpRequestMessage? request) {
            if (request == null) {
                Debug.WriteLine("null {0}", nameof(request));
                return null;
            }

            try { return await _client.SendAsync(request); }
            catch (Exception ex) {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public static HttpResponseMessage? SendRequest(HttpRequestMessage? request) {
            if (request == null) {
                Debug.WriteLine("null {0}", nameof(request));
                return null;
            }

            try { return _client.SendAsync(request).Result; }
            catch (Exception ex) {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async static Task<HttpResponseMessage?> SendRequestAsync(HttpMethod? method, Uri? uri, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (uri == null) {
                Debug.WriteLine("null {0}", nameof(uri));
                return null;
            }

            var request = new HttpRequestMessage(method, uri) { Content = content };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return await SendRequestAsync(request);
        }

        public static HttpResponseMessage? SendRequest(HttpMethod? method, Uri? uri, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (uri == null) {
                Debug.WriteLine("null {0}", nameof(uri));
                return null;
            }

            var request = new HttpRequestMessage(method, uri) { Content = content };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return SendRequest(request);
        }

        public async static Task<HttpResponseMessage?> SendRequestAsync(HttpMethod? method, string? uriString, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine("null/empty {0}", nameof(uriString));
                return null;
            }

            return await SendRequestAsync(method, new Uri(uriString), content, headers);
        }

        public static HttpResponseMessage? SendRequest(HttpMethod? method, string? uriString, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine("null/empty {0}", nameof(uriString));
                return null;
            }

            return SendRequest(method, new Uri(uriString), content, headers);
        }

        public async static Task<string?> SendRequestForStringAsync(HttpRequestMessage? request) {
            if (request == null) {
                Debug.WriteLine("null {0}", nameof(request));
                return null;
            }

            var response = await SendRequestAsync(request);
            
            if (response == null) {
                Debug.WriteLine("{0} {1} returned null response", request.Method, request.RequestUri);
                return null;
            }

            if (!response.IsSuccessStatusCode) {
                Debug.WriteLine("{0} {1} returned unsuccessfully: ({2}) {3}", request.Method, request.RequestUri, (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public static string? SendRequestForString(HttpRequestMessage? request) {
            if (request == null) {
                Debug.WriteLine("null {0}", nameof(request));
                return null;
            }

            var response = SendRequest(request);
            
            if (response == null) {
                Debug.WriteLine("{0} {1} returned null response", request.Method, request.RequestUri);
                return null;
            }

            if (!response.IsSuccessStatusCode) {
                Debug.WriteLine("{0} {1} returned unsuccessfully: ({2}) {3}", request.Method, request.RequestUri, (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public async static Task<string?> SendRequestForStringAsync(HttpMethod? method, Uri? uri, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (uri == null) {
                Debug.WriteLine("null {0}", nameof(uri));
                return null;
            }

            var request = new HttpRequestMessage(method, uri) { Content = content };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return await SendRequestForStringAsync(request);
        }

        public static string? SendRequestForString(HttpMethod? method, Uri? uri, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (uri == null) {
                Debug.WriteLine("null {0}", nameof(uri));
                return null;
            }

            var request = new HttpRequestMessage(method, uri) { Content = content };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return SendRequestForString(request);
        }

        public async static Task<string?> SendRequestForStringAsync(HttpMethod? method, string? uriString, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine("null/empty {0}", nameof(uriString));
                return null;
            }

            return await SendRequestForStringAsync(method, new Uri(uriString), content, headers);
        }

        public static string? SendRequestForString(HttpMethod? method, string? uriString, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine("null/empty {0}", nameof(uriString));
                return null;
            }

            return SendRequestForString(method, new Uri(uriString), content, headers);
        }

        public async static Task<(bool success, bool result)> SendRequestForBoolAsync(HttpRequestMessage? request) {
            if (request == null) {
                Debug.WriteLine("null {0}", nameof(request));
                return (false, false);
            }

            var responseString = await SendRequestForStringAsync(request);

            if (string.IsNullOrEmpty(responseString)) {
                Debug.WriteLine("null/empty {0}", nameof(responseString));
                return (false, false);
            }

            return (true, responseString.Equals("true", StringComparison.InvariantCultureIgnoreCase));
        }

        public static (bool success, bool result) SendRequestForBool(HttpRequestMessage? request) {
            if (request == null) {
                Debug.WriteLine("null {0}", nameof(request));
                return (false, false);
            }

            var responseString = SendRequestForString(request);

            if (string.IsNullOrEmpty(responseString)) {
                Debug.WriteLine("null/empty {0}", nameof(responseString));
                return (false, false);
            }

            return (true, responseString.Equals("true", StringComparison.InvariantCultureIgnoreCase));
        }

        public async static Task<(bool success, bool result)> SendRequestForBoolAsync(HttpMethod? method, Uri? uri, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return (false, false);
            }

            if (uri == null) {
                Debug.WriteLine("null {0}", nameof(uri));
                return (false, false);
            }

            var request = new HttpRequestMessage(method, uri) { Content = content };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return await SendRequestForBoolAsync(request);
        }

        public static (bool success, bool result) SendRequestForBool(HttpMethod? method, Uri? uri, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return (false, false);
            }

            if (uri == null) {
                Debug.WriteLine("null {0}", nameof(uri));
                return (false, false);
            }

            var request = new HttpRequestMessage(method, uri) { Content = content };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return SendRequestForBool(request);
        }

        public async static Task<(bool success, bool result)> SendRequestForBoolAsync(HttpMethod? method, string? uriString, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return (false, false);
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine("null/empty {0}", nameof(uriString));
                return (false, false);
            }

            return await SendRequestForBoolAsync(method, new Uri(uriString), content, headers);
        }

        public static (bool success, bool result) SendRequestForBool(HttpMethod? method, string? uriString, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return (false, false);
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine("null/empty {0}", nameof(uriString));
                return (false, false);
            }

            return SendRequestForBool(method, new Uri(uriString), content, headers);
        }

        public async static Task<HttpResponseMessage?> SendJsonRequestAsync(HttpMethod? method, Uri? uri, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (uri == null) {
                Debug.WriteLine("null {0}", nameof(uri));
                return null;
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine("null/empty {0}", nameof(jsonString));
                return null;
            }

            var request = new HttpRequestMessage(method, uri) { Content = new StringContent(jsonString, Encoding.UTF8, "application/json") };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return await SendRequestAsync(request);
        }

        public static HttpResponseMessage? SendJsonRequest(HttpMethod? method, Uri? uri, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (uri == null) {
                Debug.WriteLine("null {0}", nameof(uri));
                return null;
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine("null/empty {0}", nameof(jsonString));
                return null;
            }

            var request = new HttpRequestMessage(method, uri) { Content = new StringContent(jsonString, Encoding.UTF8, "application/json") };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return SendRequest(request);
        }

        public async static Task<HttpResponseMessage?> SendJsonRequestAsync(HttpMethod? method, Uri? uri, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (uri == null) {
                Debug.WriteLine("null {0}", nameof(uri));
                return null;
            }

            if (jsonObject == null) {
                Debug.WriteLine("null {0}", nameof(jsonObject));
                return null;
            }

            return await SendJsonRequestAsync(method, uri, jsonObject.ToString(), headers);
        }

        public static HttpResponseMessage? SendJsonRequest(HttpMethod? method, Uri? uri, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (uri == null) {
                Debug.WriteLine("null {0}", nameof(uri));
                return null;
            }

            if (jsonObject == null) {
                Debug.WriteLine("null {0}", nameof(jsonObject));
                return null;
            }

            return SendJsonRequest(method, uri, jsonObject.ToString(), headers);
        }

        public async static Task<HttpResponseMessage?> SendJsonRequestAsync(HttpMethod? method, string? uriString, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine("null/empty {0}", nameof(uriString));
                return null;
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine("null/empty {0}", nameof(jsonString));
                return null;
            }

            return await SendJsonRequestAsync(method, new Uri(uriString), jsonString, headers);
        }

        public static HttpResponseMessage? SendJsonRequest(HttpMethod? method, string? uriString, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine("null/empty {0}", nameof(uriString));
                return null;
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine("null/empty {0}", nameof(jsonString));
                return null;
            }

            return SendJsonRequest(method, new Uri(uriString), jsonString, headers);
        }

        public async static Task<HttpResponseMessage?> SendJsonRequestAsync(HttpMethod? method, string? uriString, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine("null/empty {0}", nameof(uriString));
                return null;
            }

            if (jsonObject == null) {
                Debug.WriteLine("null {0}", nameof(jsonObject));
                return null;
            }

            return await SendJsonRequestAsync(method, new Uri(uriString), jsonObject, headers);
        }

        public static HttpResponseMessage? SendJsonRequest(HttpMethod? method, string? uriString, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine("null/empty {0}", nameof(uriString));
                return null;
            }

            if (jsonObject == null) {
                Debug.WriteLine("null {0}", nameof(jsonObject));
                return null;
            }

            return SendJsonRequest(method, new Uri(uriString), jsonObject, headers);
        }

        public async static Task<string?> SendJsonRequestForStringAsync(HttpMethod? method, Uri? uri, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (uri == null) {
                Debug.WriteLine("null {0}", nameof(uri));
                return null;
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine("null/empty {0}", nameof(jsonString));
                return null;
            }

            var request = new HttpRequestMessage(method, uri) { Content = new StringContent(jsonString, Encoding.UTF8, "application/json") };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return await SendRequestForStringAsync(request);
        }

        public static string? SendJsonRequestForString(HttpMethod? method, Uri? uri, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (uri == null) {
                Debug.WriteLine("null {0}", nameof(uri));
                return null;
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine("null/empty {0}", nameof(jsonString));
                return null;
            }

            var request = new HttpRequestMessage(method, uri) { Content = new StringContent(jsonString, Encoding.UTF8, "application/json") };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return SendRequestForString(request);
        }

        public async static Task<string?> SendJsonRequestForStringAsync(HttpMethod? method, Uri? uri, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (uri == null) {
                Debug.WriteLine("null {0}", nameof(uri));
                return null;
            }

            if (jsonObject == null) {
                Debug.WriteLine("null {0}", nameof(jsonObject));
                return null;
            }

            return await SendJsonRequestForStringAsync(method, uri, jsonObject, headers);
        }

        public static string? SendJsonRequestForString(HttpMethod? method, Uri? uri, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (uri == null) {
                Debug.WriteLine("null {0}", nameof(uri));
                return null;
            }

            if (jsonObject == null) {
                Debug.WriteLine("null {0}", nameof(jsonObject));
                return null;
            }

            return SendJsonRequestForString(method, uri, jsonObject, headers);
        }

        public async static Task<string?> SendJsonRequestForStringAsync(HttpMethod? method, string? uriString, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine("null/empty {0}", nameof(uriString));
                return null;
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine("null/empty {0}", nameof(jsonString));
                return null;
            }

            return await SendJsonRequestForStringAsync(method, new Uri(uriString), jsonString, headers);
        }

        public static string? SendJsonRequestForString(HttpMethod? method, string? uriString, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine("null/empty {0}", nameof(uriString));
                return null;
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine("null/empty {0}", nameof(jsonString));
                return null;
            }

            return SendJsonRequestForString(method, new Uri(uriString), jsonString, headers);
        }

        public async static Task<string?> SendJsonRequestForStringAsync(HttpMethod? method, string? uriString, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine("null/empty {0}", nameof(uriString));
                return null;
            }

            if (jsonObject == null) {
                Debug.WriteLine("null {0}", nameof(jsonObject));
                return null;
            }

            return await SendJsonRequestForStringAsync(method, new Uri(uriString), jsonObject, headers);
        }

        public static string? SendJsonRequestForString(HttpMethod? method, string? uriString, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return null;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine("null/empty {0}", nameof(uriString));
                return null;
            }

            if (jsonObject == null) {
                Debug.WriteLine("null {0}", nameof(jsonObject));
                return null;
            }

            return SendJsonRequestForString(method, new Uri(uriString), jsonObject, headers);
        }

        public async static Task<(bool success, bool result)> SendJsonRequestForBoolAsync(HttpMethod? method, Uri? uri, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return (false, false);
            }

            if (uri == null) {
                Debug.WriteLine("null {0}", nameof(uri));
                return (false, false);
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine("null/empty {0}", nameof(jsonString));
                return (false, false);
            }

            var request = new HttpRequestMessage(method, uri) { Content = new StringContent(jsonString, Encoding.UTF8, "application/json") };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return await SendRequestForBoolAsync(request);
        }

        public static (bool success, bool result) SendJsonRequestForBool(HttpMethod? method, Uri? uri, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return (false, false);
            }

            if (uri == null) {
                Debug.WriteLine("null {0}", nameof(uri));
                return (false, false);
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine("null/empty {0}", nameof(jsonString));
                return (false, false);
            }

            var request = new HttpRequestMessage(method, uri) { Content = new StringContent(jsonString, Encoding.UTF8, "application/json") };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return SendRequestForBool(request);
        }

        public async static Task<(bool success, bool result)> SendJsonRequestForBoolAsync(HttpMethod? method, Uri? uri, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return (false, false);
            }

            if (uri == null) {
                Debug.WriteLine("null {0}", nameof(uri));
                return (false, false);
            }

            if (jsonObject == null) {
                Debug.WriteLine("null {0}", nameof(jsonObject));
                return (false, false);
            }

            return await SendJsonRequestForBoolAsync(method, uri, jsonObject, headers);
        }

        public static (bool success, bool result) SendJsonRequestForBool(HttpMethod? method, Uri? uri, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return (false, false);
            }

            if (uri == null) {
                Debug.WriteLine("null {0}", nameof(uri));
                return (false, false);
            }

            if (jsonObject == null) {
                Debug.WriteLine("null {0}", nameof(jsonObject));
                return (false, false);
            }

            return SendJsonRequestForBool(method, uri, jsonObject, headers);
        }

        public async static Task<(bool success, bool result)> SendJsonRequestForBoolAsync(HttpMethod? method, string? uriString, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return (false, false);
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine("null/empty {0}", nameof(uriString));
                return (false, false);
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine("null/empty {0}", nameof(jsonString));
                return (false, false);
            }

            return await SendJsonRequestForBoolAsync(method, new Uri(uriString), jsonString, headers);
        }

        public static (bool success, bool result) SendJsonRequestForBool(HttpMethod? method, string? uriString, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return (false, false);
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine("null/empty {0}", nameof(uriString));
                return (false, false);
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine("null/empty {0}", nameof(jsonString));
                return (false, false);
            }

            return SendJsonRequestForBool(method, new Uri(uriString), jsonString, headers);
        }

        public async static Task<(bool success, bool result)> SendJsonRequestForBoolAsync(HttpMethod? method, string? uriString, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return (false, false);
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine("null/empty {0}", nameof(uriString));
                return (false, false);
            }

            if (jsonObject == null) {
                Debug.WriteLine("null {0}", nameof(jsonObject));
                return (false, false);
            }

            return await SendJsonRequestForBoolAsync(method, new Uri(uriString), jsonObject, headers);
        }

        public static (bool success, bool result) SendJsonRequestForBool(HttpMethod? method, string? uriString, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine("null {0}", nameof(method));
                return (false, false);
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine("null/empty {0}", nameof(uriString));
                return (false, false);
            }

            if (jsonObject == null) {
                Debug.WriteLine("null {0}", nameof(jsonObject));
                return (false, false);
            }

            return SendJsonRequestForBool(method, new Uri(uriString), jsonObject, headers);
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    _client.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}