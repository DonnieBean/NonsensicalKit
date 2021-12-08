mergeInto(LibraryManager.library, {
   sendMessageToJS: function (key, values) {
      key = Pointer_stringify(key);
      for (var i=0;i<values.length;i++)
      { 
         if(typeof values[i]== "string")
         {
            values[i] = Pointer_stringify(values[i]);
         }
      }
      sendMessageToJS(key, values);
   },
   sendMessageToJsTest: function (key, values) {
      key = Pointer_stringify(key);
      values = Pointer_stringify(values);
      sendMessageToJsTest(key, values);
   }
});