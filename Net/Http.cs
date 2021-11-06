using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProtoSim.DotNetUtilities.Net {
    public class Http : IDisposable {
        private bool disposedValue;
        private static readonly HttpClient _client = new(new HttpClientHandler() { Proxy = null, UseProxy = false }) { BaseAddress = null, Timeout = TimeSpan.FromSeconds(10) };

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

        public async static Task<(bool success, HttpResponseMessage? response)> SendRequestAsync(HttpRequestMessage? request) {
            if (request == null) {
                Debug.WriteLine($"null {nameof(request)}");
                return default;
            }

            HttpResponseMessage? response;

            try { response = await _client.SendAsync(request); }
            catch (Exception ex) {
                Debug.WriteLine(ex);
                return default;
            }

            if (!response.IsSuccessStatusCode)
                Debug.WriteLine($"{request.Method} {request.RequestUri} returned unsuccessfully: ({(int)response.StatusCode}) {response.ReasonPhrase}", "Error");

            return (response.IsSuccessStatusCode, response);
        }

        public static (bool success, HttpResponseMessage? response) SendRequest(HttpRequestMessage? request) {
            if (request == null) {
                Debug.WriteLine($"null {nameof(request)}");
                return default;
            }

            HttpResponseMessage? response;

            try { response = _client.SendAsync(request).Result; }
            catch (Exception ex) {
                Debug.WriteLine(ex);
                return default;
            }

            if (!response.IsSuccessStatusCode)
                Debug.WriteLine($"{request.Method} {request.RequestUri} returned unsuccessfully: ({(int)response.StatusCode}) {response.ReasonPhrase}", "Error");

            return (response.IsSuccessStatusCode, response);
        }

        public async static Task<(bool success, HttpResponseMessage? response)> SendRequestAsync(HttpMethod? method, Uri? uri, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return default;
            }

            var request = new HttpRequestMessage(method, uri) { Content = content };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return await SendRequestAsync(request);
        }

        public static (bool success, HttpResponseMessage? response) SendRequest(HttpMethod? method, Uri? uri, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return default;
            }

            var request = new HttpRequestMessage(method, uri) { Content = content };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return SendRequest(request);
        }

        public async static Task<(bool success, HttpResponseMessage? response)> SendRequestAsync(HttpMethod? method, string? uriString, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return default;
            }

            return await SendRequestAsync(method, new Uri(uriString), content, headers);
        }

        public static (bool success, HttpResponseMessage? response) SendRequest(HttpMethod? method, string? uriString, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return default;
            }

            return SendRequest(method, new Uri(uriString), content, headers);
        }

        public async static Task<(bool success, string? response)> SendRequestForStringAsync(HttpRequestMessage? request) {
            if (request == null) {
                Debug.WriteLine($"null {nameof(request)}");
                return default;
            }

            var (success, response) = await SendRequestAsync(request);

            if (!success) {
                if (response == null)
                    Debug.WriteLine($"{request.Method} {request.RequestUri} returned unsuccessfully");
                else
                    Debug.WriteLine($"{request.Method} {request.RequestUri} returned unsuccessfully: ({(int)response.StatusCode}) {response.ReasonPhrase}");
            
                return default;
            }
            
            if (response == null) {
                Debug.WriteLine($"{request.Method} {request.RequestUri} returned null response");
                return default;
            }

            return (true, await response.Content.ReadAsStringAsync());
        }

        public static (bool success, string? response) SendRequestForString(HttpRequestMessage? request) {
            if (request == null) {
                Debug.WriteLine($"null {nameof(request)}");
                return default;
            }

            var (success, response) = SendRequest(request);

            if (!success) {
                if (response == null)
                    Debug.WriteLine($"{request.Method} {request.RequestUri} returned unsuccessfully");
                else
                    Debug.WriteLine($"{request.Method} {request.RequestUri} returned unsuccessfully: ({(int)response.StatusCode}) {response.ReasonPhrase}");

                return default;
            }

            if (response == null) {
                Debug.WriteLine($"{request.Method} {request.RequestUri} returned null response");
                return default;
            }

            return (true, response.Content.ReadAsStringAsync().Result);
        }

        public async static Task<(bool success, string? response)> SendRequestForStringAsync(HttpMethod? method, Uri? uri, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return default;
            }

            var request = new HttpRequestMessage(method, uri) { Content = content };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return await SendRequestForStringAsync(request);
        }

        public static (bool success, string? response) SendRequestForString(HttpMethod? method, Uri? uri, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return default;
            }

            var request = new HttpRequestMessage(method, uri) { Content = content };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return SendRequestForString(request);
        }

        public async static Task<(bool success, string? response)> SendRequestForStringAsync(HttpMethod? method, string? uriString, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return default;
            }

            return await SendRequestForStringAsync(method, new Uri(uriString), content, headers);
        }

        public static (bool success, string? response) SendRequestForString(HttpMethod? method, string? uriString, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return default;
            }

            return SendRequestForString(method, new Uri(uriString), content, headers);
        }

        public async static Task<(bool success, bool response)> SendRequestForBoolAsync(HttpRequestMessage? request) {
            if (request == null) {
                Debug.WriteLine($"null {nameof(request)}");
                return (false, false);
            }

            var (success, response) = await SendRequestForStringAsync(request);

            if (!success) {
                if (response == null)
                    Debug.WriteLine($"{request.Method} {request.RequestUri} returned unsuccessfully");
                else
                    Debug.WriteLine($"{request.Method} {request.RequestUri} returned unsuccessfully: {response}");

                return default;
            }

            if (response == null) {
                Debug.WriteLine($"{request.Method} {request.RequestUri} returned null response");
                return default;
            }

            return (true, response.Equals("true", StringComparison.InvariantCultureIgnoreCase));
        }

        public static (bool success, bool response) SendRequestForBool(HttpRequestMessage? request) {
            if (request == null) {
                Debug.WriteLine($"null {nameof(request)}");
                return (false, false);
            }

            var (success, response) = SendRequestForString(request);

            if (!success) {
                if (response == null)
                    Debug.WriteLine($"{request.Method} {request.RequestUri} returned unsuccessfully");
                else
                    Debug.WriteLine($"{request.Method} {request.RequestUri} returned unsuccessfully: {response}");

                return default;
            }

            if (response == null) {
                Debug.WriteLine($"{request.Method} {request.RequestUri} returned null response");
                return default;
            }

            return (true, response.Equals("true", StringComparison.InvariantCultureIgnoreCase));
        }

        public async static Task<(bool success, bool response)> SendRequestForBoolAsync(HttpMethod? method, Uri? uri, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return (false, false);
            }

            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return (false, false);
            }

            var request = new HttpRequestMessage(method, uri) { Content = content };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return await SendRequestForBoolAsync(request);
        }

        public static (bool success, bool response) SendRequestForBool(HttpMethod? method, Uri? uri, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return (false, false);
            }

            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return (false, false);
            }

            var request = new HttpRequestMessage(method, uri) { Content = content };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return SendRequestForBool(request);
        }

        public async static Task<(bool success, bool response)> SendRequestForBoolAsync(HttpMethod? method, string? uriString, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return (false, false);
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return (false, false);
            }

            return await SendRequestForBoolAsync(method, new Uri(uriString), content, headers);
        }

        public static (bool success, bool response) SendRequestForBool(HttpMethod? method, string? uriString, HttpContent? content = null, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return (false, false);
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return (false, false);
            }

            return SendRequestForBool(method, new Uri(uriString), content, headers);
        }

        public async static Task<(bool success, HttpResponseMessage? response)> SendJsonRequestAsync(HttpMethod? method, Uri? uri, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return default;
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine($"null/empty {nameof(jsonString)}");
                return default;
            }

            var request = new HttpRequestMessage(method, uri) { Content = new StringContent(jsonString, Encoding.UTF8, "application/json") };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return await SendRequestAsync(request);
        }

        public static (bool success, HttpResponseMessage? response) SendJsonRequest(HttpMethod? method, Uri? uri, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return default;
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine($"null/empty {nameof(jsonString)}");
                return default;
            }

            var request = new HttpRequestMessage(method, uri) { Content = new StringContent(jsonString, Encoding.UTF8, "application/json") };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return SendRequest(request);
        }

        public async static Task<(bool success, HttpResponseMessage? response)> SendJsonRequestAsync(HttpMethod? method, Uri? uri, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return default;
            }

            if (jsonObject == null) {
                Debug.WriteLine($"null {nameof(jsonObject)}");
                return default;
            }

            return await SendJsonRequestAsync(method, uri, jsonObject.ToString(), headers);
        }

        public static (bool success, HttpResponseMessage? response) SendJsonRequest(HttpMethod? method, Uri? uri, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return default;
            }

            if (jsonObject == null) {
                Debug.WriteLine($"null {nameof(jsonObject)}");
                return default;
            }

            return SendJsonRequest(method, uri, jsonObject.ToString(), headers);
        }

        public async static Task<(bool success, HttpResponseMessage? response)> SendJsonRequestAsync(HttpMethod? method, string? uriString, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return default;
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine($"null/empty {nameof(jsonString)}");
                return default;
            }

            return await SendJsonRequestAsync(method, new Uri(uriString), jsonString, headers);
        }

        public static (bool success, HttpResponseMessage? response) SendJsonRequest(HttpMethod? method, string? uriString, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return default;
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine($"null/empty {nameof(jsonString)}");
                return default;
            }

            return SendJsonRequest(method, new Uri(uriString), jsonString, headers);
        }

        public async static Task<(bool success, HttpResponseMessage? response)> SendJsonRequestAsync(HttpMethod? method, string? uriString, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return default;
            }

            if (jsonObject == null) {
                Debug.WriteLine($"null {nameof(jsonObject)}");
                return default;
            }

            return await SendJsonRequestAsync(method, new Uri(uriString), jsonObject, headers);
        }

        public static (bool success, HttpResponseMessage? response) SendJsonRequest(HttpMethod? method, string? uriString, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return default;
            }

            if (jsonObject == null) {
                Debug.WriteLine($"null {nameof(jsonObject)}");
                return default;
            }

            return SendJsonRequest(method, new Uri(uriString), jsonObject, headers);
        }

        public async static Task<(bool success, string? response)> SendJsonRequestForStringAsync(HttpMethod? method, Uri? uri, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return default;
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine($"null/empty {nameof(jsonString)}");
                return default;
            }

            var request = new HttpRequestMessage(method, uri) { Content = new StringContent(jsonString, Encoding.UTF8, "application/json") };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return await SendRequestForStringAsync(request);
        }

        public static (bool success, string? response) SendJsonRequestForString(HttpMethod? method, Uri? uri, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return default;
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine($"null/empty {nameof(jsonString)}");
                return default;
            }

            var request = new HttpRequestMessage(method, uri) { Content = new StringContent(jsonString, Encoding.UTF8, "application/json") };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return SendRequestForString(request);
        }

        public async static Task<(bool success, string? response)> SendJsonRequestForStringAsync(HttpMethod? method, Uri? uri, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return default;
            }

            if (jsonObject == null) {
                Debug.WriteLine($"null {nameof(jsonObject)}");
                return default;
            }

            return await SendJsonRequestForStringAsync(method, uri, jsonObject, headers);
        }

        public static (bool success, string? response) SendJsonRequestForString(HttpMethod? method, Uri? uri, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return default;
            }

            if (jsonObject == null) {
                Debug.WriteLine($"null {nameof(jsonObject)}");
                return default;
            }

            return SendJsonRequestForString(method, uri, jsonObject, headers);
        }

        public async static Task<(bool success, string? response)> SendJsonRequestForStringAsync(HttpMethod? method, string? uriString, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return default;
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine($"null/empty {nameof(jsonString)}");
                return default;
            }

            return await SendJsonRequestForStringAsync(method, new Uri(uriString), jsonString, headers);
        }

        public static (bool success, string? response) SendJsonRequestForString(HttpMethod? method, string? uriString, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return default;
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine($"null/empty {nameof(jsonString)}");
                return default;
            }

            return SendJsonRequestForString(method, new Uri(uriString), jsonString, headers);
        }

        public async static Task<(bool success, string? response)> SendJsonRequestForStringAsync(HttpMethod? method, string? uriString, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return default;
            }

            if (jsonObject == null) {
                Debug.WriteLine($"null {nameof(jsonObject)}");
                return default;
            }

            return await SendJsonRequestForStringAsync(method, new Uri(uriString), jsonObject, headers);
        }

        public static (bool success, string? response) SendJsonRequestForString(HttpMethod? method, string? uriString, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return default;
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return default;
            }

            if (jsonObject == null) {
                Debug.WriteLine($"null {nameof(jsonObject)}");
                return default;
            }

            return SendJsonRequestForString(method, new Uri(uriString), jsonObject, headers);
        }

        public async static Task<(bool success, bool response)> SendJsonRequestForBoolAsync(HttpMethod? method, Uri? uri, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return (false, false);
            }

            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return (false, false);
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine($"null/empty {nameof(jsonString)}");
                return (false, false);
            }

            var request = new HttpRequestMessage(method, uri) { Content = new StringContent(jsonString, Encoding.UTF8, "application/json") };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return await SendRequestForBoolAsync(request);
        }

        public static (bool success, bool response) SendJsonRequestForBool(HttpMethod? method, Uri? uri, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return (false, false);
            }

            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return (false, false);
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine($"null/empty {nameof(jsonString)}");
                return (false, false);
            }

            var request = new HttpRequestMessage(method, uri) { Content = new StringContent(jsonString, Encoding.UTF8, "application/json") };

            if (headers != null) {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return SendRequestForBool(request);
        }

        public async static Task<(bool success, bool response)> SendJsonRequestForBoolAsync(HttpMethod? method, Uri? uri, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return (false, false);
            }

            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return (false, false);
            }

            if (jsonObject == null) {
                Debug.WriteLine($"null {nameof(jsonObject)}");
                return (false, false);
            }

            return await SendJsonRequestForBoolAsync(method, uri, jsonObject, headers);
        }

        public static (bool success, bool response) SendJsonRequestForBool(HttpMethod? method, Uri? uri, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return (false, false);
            }

            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return (false, false);
            }

            if (jsonObject == null) {
                Debug.WriteLine($"null {nameof(jsonObject)}");
                return (false, false);
            }

            return SendJsonRequestForBool(method, uri, jsonObject, headers);
        }

        public async static Task<(bool success, bool response)> SendJsonRequestForBoolAsync(HttpMethod? method, string? uriString, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return (false, false);
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return (false, false);
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine($"null/empty {nameof(jsonString)}");
                return (false, false);
            }

            return await SendJsonRequestForBoolAsync(method, new Uri(uriString), jsonString, headers);
        }

        public static (bool success, bool response) SendJsonRequestForBool(HttpMethod? method, string? uriString, string? jsonString, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return (false, false);
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return (false, false);
            }

            if (string.IsNullOrEmpty(jsonString)) {
                Debug.WriteLine($"null/empty {nameof(jsonString)}");
                return (false, false);
            }

            return SendJsonRequestForBool(method, new Uri(uriString), jsonString, headers);
        }

        public async static Task<(bool success, bool response)> SendJsonRequestForBoolAsync(HttpMethod? method, string? uriString, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return (false, false);
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return (false, false);
            }

            if (jsonObject == null) {
                Debug.WriteLine($"null {nameof(jsonObject)}");
                return (false, false);
            }

            return await SendJsonRequestForBoolAsync(method, new Uri(uriString), jsonObject, headers);
        }

        public static (bool success, bool response) SendJsonRequestForBool(HttpMethod? method, string? uriString, object? jsonObject, List<KeyValuePair<string, string>>? headers = null) {
            if (method == null) {
                Debug.WriteLine($"null {nameof(method)}");
                return (false, false);
            }

            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return (false, false);
            }

            if (jsonObject == null) {
                Debug.WriteLine($"null {nameof(jsonObject)}");
                return (false, false);
            }

            return SendJsonRequestForBool(method, new Uri(uriString), jsonObject, headers);
        }

        public async static Task<bool> DownloadFileAsync(Uri? uri, string? filepath) {
            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return false;
            }

            if (string.IsNullOrEmpty(filepath)) {
                Debug.WriteLine($"null/empty {nameof(filepath)}");
                return false;
            }

            byte[] fileBytes = await _client.GetByteArrayAsync(uri);

            if (fileBytes == null) {
                Debug.WriteLine($"null {nameof(fileBytes)}");
                return false;
            }

            if (fileBytes.Length < 1) {
                Debug.WriteLine($"empty {nameof(fileBytes)}");
                return false;
            }

            await File.WriteAllBytesAsync(filepath, fileBytes);
            return File.Exists(filepath);
        }

        public static bool DownloadFile(Uri? uri, string? filepath) {
            if (uri == null) {
                Debug.WriteLine($"null {nameof(uri)}");
                return false;
            }

            if (string.IsNullOrEmpty(filepath)) {
                Debug.WriteLine($"null/empty {nameof(filepath)}");
                return false;
            }

            byte[] fileBytes = Task.Run(() => _client.GetByteArrayAsync(uri)).Result;

            if (fileBytes == null) {
                Debug.WriteLine($"null {nameof(fileBytes)}");
                return false;
            }

            if (fileBytes.Length < 1) {
                Debug.WriteLine($"empty {nameof(fileBytes)}");
                return false;
            }

            File.WriteAllBytes(filepath, fileBytes);
            return File.Exists(filepath);
        }

        public async static Task<bool> DownloadFileAsync(string? uriString, string? filepath) {
            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return false;
            }

            if (string.IsNullOrEmpty(filepath)) {
                Debug.WriteLine($"null/empty {nameof(filepath)}");
                return false;
            }

            byte[] fileBytes = await _client.GetByteArrayAsync(uriString);

            if (fileBytes == null) {
                Debug.WriteLine($"null {nameof(fileBytes)}");
                return false;
            }

            if (fileBytes.Length < 1) {
                Debug.WriteLine($"empty {nameof(fileBytes)}");
                return false;
            }

            await File.WriteAllBytesAsync(filepath, fileBytes);
            return File.Exists(filepath);
        }

        public static bool DownloadFile(string? uriString, string? filepath) {
            if (string.IsNullOrEmpty(uriString)) {
                Debug.WriteLine($"null/empty {nameof(uriString)}");
                return false;
            }

            if (string.IsNullOrEmpty(filepath)) {
                Debug.WriteLine($"null/empty {nameof(filepath)}");
                return false;
            }

            byte[] fileBytes = Task.Run(() => _client.GetByteArrayAsync(uriString)).Result;

            if (fileBytes == null) {
                Debug.WriteLine($"null {nameof(fileBytes)}");
                return false;
            }

            if (fileBytes.Length < 1) {
                Debug.WriteLine($"empty {nameof(fileBytes)}");
                return false;
            }

            File.WriteAllBytes(filepath, fileBytes);
            return File.Exists(filepath);
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