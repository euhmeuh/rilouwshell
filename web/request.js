"use strict";
const Request = (function(){

  const MIME_JSON = 'application/json';

  function ajax(method, url, success, failure, contentType, data) {
    var request = new XMLHttpRequest();
    request.open(method, url, true);
    if (contentType) {
      request.setRequestHeader('Content-Type', contentType);
    }
    request.onreadystatechange = function () {
      if (request.readyState === XMLHttpRequest.DONE) {
        if (request.status === 200) {
          success(request.responseText)
        } else {
          failure(request.status, request.statusText, request.responseText);
        }
      }
    }
    return request.send(data);
  }

  function formatParams(params) {
    return "?" + _.pairs(params)
      .map(param => encodeURIComponent(param.key) + "=" + encodeURIComponent(param.value))
      .join("&");
  }

  function get(url, params) {
    return new Promise((resolve, reject) => {
      ajax(
        'GET',
        url + formatParams(params),
        (response) => resolve(JSON.parse(response)),
        (code, status, response) => reject({ code, status, response })
      );
    });
  }

  function post(url, data, contentType) {
    return new Promise((resolve, reject) => {
      ajax(
        'POST',
        url,
        (response) => resolve(JSON.parse(response)),
        (code, status, response) => reject({ code, status, response }),
        contentType || MIME_JSON,
        contentType === MIME_JSON ? JSON.stringify(data) : data,
      );
    });
  }

  return { get, post };
}());
