var cefsharp;
if (!cefsharp)
    cefsharp = {};

if (!cefsharp.example)
    cefsharp.example = {};

(function ()
{
    cefsharp.example.alert = function(text)
    {
        alert(text);
    };
})();