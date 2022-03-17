mergeInto(LibraryManager.library, {
   sendMessageToJS: function (key, values) {
      key = Pointer_stringify(key);
      values = Pointer_stringify(values);
      sendMessageToJS(key, values);
   },
   sendMessageToJsTest: function (key, values) {
      key = Pointer_stringify(key);
      values = Pointer_stringify(values);
      sendMessageToJsTest(key, values);
   }
});