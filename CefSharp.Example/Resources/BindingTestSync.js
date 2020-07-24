(async () =>
{
    await CefSharp.BindObjectAsync("bound");

    QUnit.test("BindObjectAsync Second call with Bound param", function (assert)
    {
        let asyncCallback = assert.async();
        CefSharp.BindObjectAsync("bound").then(function (res)
        {
            assert.equal(res.Success, false, "Second call to BindObjectAsync with already bound objects as params returned false.");
            asyncCallback();
        });
    });

    QUnit.test("bound.repeat('hi ', 5)", function (assert)
    {
        const expectedResult = "hi hi hi hi hi "
        let actualResult = bound.repeat("hi ", 5);
        assert.equal(actualResult, expectedResult, "We expect value to be " + actualResult);
    });

    QUnit.test("bound.myProperty", function (assert)
    {
        const expectedResult = 42;
        let actualResult = bound.myProperty;

        assert.equal(actualResult, expectedResult, "We expect value to be " + expectedResult);
    });

    QUnit.test("bound.subObject.simpleProperty", function (assert)
    {
        const expectedResult = "This is a very simple property.";
        let actualResult = bound.subObject.simpleProperty;

        assert.equal(actualResult, expectedResult, "We expect value to be " + expectedResult);

        bound.subObject.simpleProperty = expectedResult + "1";

        actualResult = bound.subObject.simpleProperty;

        assert.equal(actualResult, (expectedResult + "1"), "We expect value to be " + (expectedResult + 1));

        //Reset to default value
        bound.subObject.simpleProperty = expectedResult;
    });

    QUnit.test("Function delegate to c# method", function (assert)
    {
        function myFunction(functionParam)
        {
            return functionParam();
        }

        const expectedResult = "42"
        let actualResult = myFunction(bound.echoMyProperty);
        assert.equal(actualResult, expectedResult, "We expect value to be " + actualResult);
    });

    QUnit.test("Function returning complex type", function (assert)
    {
        function myFunction(functionParam)
        {
            return functionParam();
        }

        const expectedResult = "This is a very simple property."
        let actualResult = bound.getSubObject().simpleProperty;
        assert.equal(actualResult, expectedResult, "Call to bound.getSubObject().simpleProperty resulted in : " + actualResult);
    });

    QUnit.test("Stress Test", function (assert)
    {
        let stressTestCallCount = 1000;
        for (var i = 1; i <= stressTestCallCount; i++)
        {
            assert.ok(true, bound.repeat("hi ", 5));
        }

        assert.ok(true, "Stress Test done with : " + stressTestCallCount + " call to bound.repeat(\"hi \", 5)");
    });

    QUnit.test("JSON Serializer Test", function (assert)
    {
        var json = bound.returnJsonEmployeeList();
        var jsonObj = JSON.parse(json);

        assert.ok(jsonObj.employees.length > 0, "Employee Count : " + jsonObj.employees.length);

        var employees = jsonObj.employees;

        assert.equal("John Doe", employees[0].firstName + " " + employees[0].lastName, "Employee : " + employees[0].firstName + " " + employees[0].lastName);
        assert.equal("Anna Smith", employees[1].firstName + " " + employees[1].lastName, "Employee : " + employees[1].firstName + " " + employees[1].lastName);
        assert.equal("Peter Jones", employees[2].firstName + " " + employees[2].lastName, "Employee : " + employees[2].firstName + " " + employees[2].lastName);
    });

    QUnit.test("Javascript function sending variable number of parameters to Bound Object. (ParamArray Test)", function (assert)
    {
        assert.equal(bound.methodWithParams('With 1 Params', 'hello-world'), "Name:With 1 Params;Args:hello-world", "Method with paramArray with one param");
        assert.equal(bound.methodWithParams('With 2 Params', 'hello-world', 'chris was here'), "Name:With 2 Params;Args:hello-world, chris was here", "Method with paramArray with two params");
        assert.equal(bound.methodWithParams('With no Params'), "Name:With no Params;Args:", "Method with paramArary no params passed");
        assert.equal(bound.methodWithoutParams('Normal Method', 'hello'), "Normal Method, hello", "Method with one param (not param array)");
        assert.equal(bound.methodWithoutAnything(), "Method without anything called and returned successfully.", "Method without params");

        assert.equal(bound.methodWithThreeParamsOneOptionalOneArray(null), "MethodWithThreeParamsOneOptionalOneArray:No Name Specified - No Optional Param Specified;Args:", "bound.methodWithThreeParamsOneOptionalOneArray(null)");
        assert.equal(bound.methodWithThreeParamsOneOptionalOneArray(null, null), "MethodWithThreeParamsOneOptionalOneArray:No Name Specified - No Optional Param Specified;Args:", "bound.methodWithThreeParamsOneOptionalOneArray(null, null)");
        assert.equal(bound.methodWithThreeParamsOneOptionalOneArray("Test", null), "MethodWithThreeParamsOneOptionalOneArray:Test - No Optional Param Specified;Args:", "bound.methodWithThreeParamsOneOptionalOneArray('Test', null)");
        assert.equal(bound.methodWithThreeParamsOneOptionalOneArray(null, null, "Arg1", "Arg2"), "MethodWithThreeParamsOneOptionalOneArray:No Name Specified - No Optional Param Specified;Args:Arg1, Arg2", "bound.methodWithThreeParamsOneOptionalOneArray(null, null, 'Arg1', 'Arg2')");
    });

    QUnit.test("Methods and Properties on bound object 'bound':", function (assert)
    {
        for (var name in bound)
        {
            if (bound[name].constructor.name != 'Function') continue;

            assert.ok(true, "Property: " + name);
        }

        for (var name in bound)
        {
            if (bound[name].constructor.name === 'Function') continue;


            assert.ok(true, "Function: " + name);
        }

        for (var name in bound)
        {
            if (typeof bound[name] == "object" && bound[name] !== null)
            {
                for (var sub in bound[name])
                {
                    var type = bound[name][sub].constructor.name === 'Function' ? "Function" : "Property";
                    assert.ok(true, name + "." + sub + "(" + type + ")");
                }
            }
        }
    });

    QUnit.test("Javascript Callback Test using object as parameter", function (assert)
    {
        var asyncCallback = assert.async();

        function objectCallback(s)
        {
            assert.ok(true, s);

            asyncCallback();
        }

        bound.testCallbackFromObject(
            {
                Callback: objectCallback, TestString: "Hello", SubClasses:
                    [
                        { PropertyOne: "Test Property One", Numbers: [1, 2, 3] },
                        { PropertyOne: "Test Property Two", Numbers: [4, 5, 6] }
                    ]
            });
    });

    QUnit.test("Javascript Callback Test with DateTime", function (assert)
    {
        var asyncCallback = assert.async();

        function callback(dateTimeFirst, dateTimeSecondAsArray)
        {
            var arr = dateTimeSecondAsArray;
            var dateTimeSecond = new Date(arr[0], arr[1] - 1, arr[2], arr[3], arr[4], arr[5]);
            assert.equal(dateTimeFirst - dateTimeSecond, 0);

            asyncCallback();
        }

        bound.testCallbackWithDateTime(callback);
    });

    QUnit.test("Javascript Callback Test with 1900 DateTime", function (assert)
    {
        var asyncCallback = assert.async();

        function callback(dateTimeFirst, dateTimeSecondAsArray)
        {
            var arr = dateTimeSecondAsArray;
            var dateTimeSecond = new Date(arr[0], arr[1] - 1, arr[2], arr[3], arr[4], arr[5]);
            assert.equal(dateTimeFirst - dateTimeSecond, 0);

            asyncCallback();
        }

        bound.testCallbackWithDateTime1900(callback);
    });

    QUnit.test("Javascript Callback Test with 1970 DateTime", function (assert)
    {
        var asyncCallback = assert.async();

        function callback(dateTimeFirst, dateTimeSecondAsArray)
        {
            var arr = dateTimeSecondAsArray;
            var dateTimeSecond = new Date(arr[0], arr[1] - 1, arr[2], arr[3], arr[4], arr[5]);
            assert.equal(dateTimeFirst - dateTimeSecond, 0);

            asyncCallback();
        }

        bound.testCallbackWithDateTime1970(callback);
    });

    QUnit.test("Javascript Callback Test with 1985 DateTime", function (assert)
    {
        var asyncCallback = assert.async();

        function callback(dateTimeFirst, dateTimeSecondAsArray)
        {
            var arr = dateTimeSecondAsArray;
            var dateTimeSecond = new Date(arr[0], arr[1] - 1, arr[2], arr[3], arr[4], arr[5]);
            assert.equal(dateTimeFirst - dateTimeSecond, 0);

            asyncCallback();
        }

        bound.testCallbackWithDateTime1985(callback);
    });

    QUnit.test("Javascript Callback Test", function (assert)
    {
        var asyncCallback = assert.async();

        function callback(s)
        {
            assert.equal(s.Response, "This callback from C# was delayed 1500ms", s.Response);

            asyncCallback();
        }

        bound.testCallback(callback);
    });
})();
