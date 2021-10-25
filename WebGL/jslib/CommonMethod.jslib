mergeInto(LibraryManager.library, {
   sendMessageToJs: function (key, values) {
      key = Pointer_stringify(key);
      for (var i=0;i<values.length;i++)
      { 
         values[i] = Pointer_stringify(values[i]);
      }
      sendMessageToJs(key, values);
   }
});