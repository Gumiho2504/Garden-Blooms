mergeInto(LibraryManager.library, {
  Hello: function () {
    window.alert("Hello, world!");
  },

  HelloString: function (str) {
    window.alert(UTF8ToString(str));
  },

  SendMessageToFlutter: function (str) {
    if (window.toast) {
      window.toast.postMessage(UTF8ToString(str));
    }
  },
});
