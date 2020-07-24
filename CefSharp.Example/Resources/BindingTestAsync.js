(async () =>
{
    // Verify that two objects are completely equal
    function deepEqual(x, y)
    {
        if ((typeof x == "object" && x != null) && (typeof y == "object" && y != null))
        {
            for (var prop in x)
            {
                if (prop in y && (Object.keys(x).length === Object.keys(y).length))
                {
                    return deepEqual(x[prop], y[prop]);
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            return x === y;
        }
    }

    await CefSharp.BindObjectAsync("boundAsync");

    QUnit.test("BindObjectAsync Second call with boundAsync param", function (assert)
    {
        let asyncCallback = assert.async();
        CefSharp.BindObjectAsync("boundAsync").then(function (res)
        {
            assert.equal(res.Success, false, "Second call to BindObjectAsync with already bound objects as params returned false.");
            asyncCallback();
        });
    });

    QUnit.test("Async call (Throw .Net Exception)", function (assert)
    {
        var asyncCallback = assert.async();

        boundAsync.error().catch(function (e)
        {
            assert.ok(true, "Error: " + e);

            asyncCallback();
        });
    });

    QUnit.test("Async call (Divide 16 / 2):", function (assert)
    {
        var asyncCallback = assert.async();

        boundAsync.div(16, 2).then(function (actualResult)
        {
            const expectedResult = 8

            assert.equal(expectedResult, actualResult, "Divide 16 / 2 resulted in " + expectedResult);

            asyncCallback();
        });
    });

    QUnit.test("Async call (Divide 16 /0)", function (assert)
    {
        var asyncCallback = assert.async();

        boundAsync.div(16, 0).then(function (res)
        {
            //Will always throw exception
        },
            function (e)
            {
                assert.ok(true, "Error: " + e + "(" + Date() + ")");

                asyncCallback();
            });
    });

    QUnit.test("Async call (UIntAddModel 3 + 2):", function (assert)
    {
        var asyncCallback = assert.async();

        boundAsync.uIntAddModel({ ParamA: 3, ParamB: 2 }).then(function (actualResult)
        {
            const expectedResult = 5

            assert.equal(expectedResult, actualResult, "Add 3 + 2 resulted in " + expectedResult);

            asyncCallback();
        });
    });

    QUnit.test("Async call (UIntAdd 3 + 2):", function (assert)
    {
        var asyncCallback = assert.async();

        boundAsync.uIntAdd(3, 2).then(function (actualResult)
        {
            const expectedResult = 5

            assert.equal(expectedResult, actualResult, "Add 3 + 2 resulted in " + expectedResult);

            asyncCallback();
        });
    });

    QUnit.test("Async call (Hello):", function (assert)
    {
        var asyncCallback = assert.async();

        boundAsync.hello('CefSharp').then(function (res)
        {
            assert.equal(res, "Hello CefSharp")

            asyncCallback();
        });
    });

    QUnit.test("Async call (Long Running Task):", function (assert)
    {
        var asyncCallback = assert.async();

        boundAsync.doSomething().then(function (res)
        {
            assert.ok(true, "Slept for 1000ms")

            asyncCallback();
        });
    });

    QUnit.test("Async call (return Struct):", function (assert)
    {
        var asyncCallback = assert.async();

        boundAsync.returnObject('CefSharp Struct Test').then(function (res)
        {
            assert.equal(res.Value, "CefSharp Struct Test", "Struct with a single field");

            asyncCallback();
        });
    });

    QUnit.test("Async call (return Class):", function (assert)
    {
        var asyncCallback = assert.async();

        //Returns a class
        boundAsync.returnClass('CefSharp Class Test').then(function (res)
        {
            const expectedResult = 'CefSharp Class Test';

            assert.equal(expectedResult, res.Value, "Class with a single property");

            asyncCallback();
        });
    });

    QUnit.test("Async call (return Class as JsonString):", function (assert)
    {
        var asyncCallback = assert.async();

        const expectedResult = 'CefSharp Class Test';

        //Returns a class
        boundAsync.returnClassAsJsonString(expectedResult).then(function (res)
        {
            assert.equal(expectedResult, res.Value, "Class with a single property");

            asyncCallback();
        });
    });

    QUnit.test("Async call (returnStructArray):", function (assert)
    {
        var asyncCallback = assert.async();

        boundAsync.returnStructArray('CefSharp').then(function (res)
        {
            assert.equal(res[0].Value, "CefSharpItem1", "Expected Result of CefSharpItem1");
            assert.equal(res[1].Value, "CefSharpItem2", "Expected Result of CefSharpItem2");

            asyncCallback();
        });
    });

    QUnit.test("Async call (returnClassesArray):", function (assert)
    {
        var asyncCallback = assert.async();

        boundAsync.returnClassesArray('CefSharp').then(function (res)
        {
            assert.equal(res[0].Value, "CefSharpItem1", "Expected Result of CefSharpItem1");
            assert.equal(res[1].Value, "CefSharpItem2", "Expected Result of CefSharpItem2");

            asyncCallback();
        });
    });

    QUnit.test("Async call (echoArray):", function (assert)
    {
        var asyncCallback = assert.async();

        boundAsync.echoArray(["one", null, "three"]).then(function (res)
        {
            assert.equal(res.length, 3, "Expected result to be length 3");
            assert.equal(res[0], "one", "Expected Result of 1st item");
            assert.equal(res[1], null, "Expected Result of 2nd item");
            assert.equal(res[2], "three", "Expected Result of 3rd item");

            asyncCallback();
        });
    });

    QUnit.test("Async call (Test Empty Array):", function (assert)
    {
        var asyncCallback = assert.async();

        boundAsync.echoArray([]).then(function (res)
        {
            assert.equal(Array.isArray(res), true, "Expected result is an array");
            assert.equal(res.length, 0, "Expected result to be length 0 (empty array)");
            asyncCallback();
        });
    });


    QUnit.test("Async call (echoValueTypeArray):", function (assert)
    {
        var asyncCallback = assert.async();

        boundAsync.echoValueTypeArray([1, null, 3]).then(function (res)
        {
            assert.equal(res.length, 3, "Expected result to be length 3");
            assert.equal(res[0], 1, "Expected Result of 1st item");
            assert.equal(res[1], 0, "Expected Result of 2nd item");
            assert.equal(res[2], 3, "Expected Result of 3rd item");

            asyncCallback();
        });
    });

    QUnit.test("Async call (echoMultidimensionalArray):", function (assert)
    {
        var asyncCallback = assert.async();

        boundAsync.echoMultidimensionalArray([[1, 2], null, [3, 4]]).then(function (res)
        {
            assert.equal(res.length, 3, "Expected result to be length 3");
            assert.equal(res[0][0], 1, "Expected Result of 1st item");
            assert.equal(res[0][1], 2, "Expected Result of 1st item");
            assert.equal(res[1], null, "Expected Result of 2nd item");
            assert.equal(res[2][0], 3, "Expected Result of 3rd item");
            assert.equal(res[2][1], 4, "Expected Result of 3rd item");

            asyncCallback();
        });
    });

    QUnit.test("Async call (methodReturnList):", function (assert)
    {
        const list1 =
            [
                "Element 0 - First",
                "Element 1",
                "Element 2 - Last"
            ];

        const list2 =
            [
                ["Element 0, 0", "Element 0, 1"],
                ["Element 1, 0", "Element 1, 1"],
                ["Element 2, 0", "Element 2, 1"]
            ];

        var asyncCallback = assert.async();
        Promise.all([
            boundAsync.methodReturnsList(),
            boundAsync.methodReturnsListOfLists(),
        ]).then(function (results)
        {
            assert.ok(deepEqual(results[0], list1), "Call to boundAsync.MethodReturnsList() resulted in : " + JSON.stringify(results[0]));
            assert.ok(deepEqual(results[1], list2), "Call to boundAsync.MethodReturnsListOfLists() resulted in : " + JSON.stringify(results[1]));
            asyncCallback();
        });

    });

    QUnit.test("Async call (methodReturnsDictionary):", function (assert)
    {
        const dict1 =
        {
            "five": 5,
            "ten": 10
        };
        const dict2 =
        {
            "onepointfive": 1.5,
            "five": 5,
            "ten": "ten",
            "twotwo": [2, 2]
        };
        const dict3 =
        {
            "data":
            {
                "onepointfive": 1.5,
                "five": 5,
                "ten": "ten",
                "twotwo": [2, 2]
            }
        };

        var asyncCallback = assert.async();
        Promise.all([
            boundAsync.methodReturnsDictionary1(),
            boundAsync.methodReturnsDictionary2(),
            boundAsync.methodReturnsDictionary3()
        ]).then(function (results)
        {
            assert.ok(deepEqual(results[0], dict1), "Call to boundAsync.MethodReturnsDictionary1() resulted in : " + JSON.stringify(results[0]));
            assert.ok(deepEqual(results[1], dict2), "Call to boundAsync.MethodReturnsDictionary2() resulted in : " + JSON.stringify(results[1]));
            assert.ok(deepEqual(results[2], dict3), "Call to boundAsync.MethodReturnsDictionary3() resulted in : " + JSON.stringify(results[2]));
            asyncCallback();
        });
    });

    QUnit.test("Bind boundAsync2 and call (Hello):", function (assert)
    {
        var asyncCallback = assert.async();

        //Can use both cefSharp.bindObjectAsync and CefSharp.BindObjectAsync, both do the same
        cefSharp.bindObjectAsync({ NotifyIfAlreadyBound: true }, "boundAsync2").then(function (result)
        {
            boundAsync2.hello('CefSharp').then(function (res)
            {
                assert.equal(res, "Hello CefSharp")

                assert.equal(true, CefSharp.DeleteBoundObject("boundAsync2"), "Object was unbound");

                assert.ok(window.boundAsync2 === undefined, "boundAsync2 is now undefined");

                asyncCallback();
            });
        });
    });

    //Repeat of the previous test to make sure binding a deleted object is working
    QUnit.test("Bind boundAsync2 and call (Hello) Second Attempt:", function (assert)
    {
        var asyncCallback = assert.async();

        CefSharp.BindObjectAsync({ NotifyIfAlreadyBound: true, IgnoreCache: true }, "boundAsync2").then(function (result)
        {
            boundAsync2.hello('CefSharp').then(function (res)
            {
                assert.equal(res, "Hello CefSharp")

                asyncCallback();
            });
        });
    });

    //Repeat of the previous test to make sure binding a deleted object is working
    QUnit.test("Bind boundAsync2 and call (Hello) Third Attempt:", function (assert)
    {
        var asyncCallback = assert.async();

        CefSharp.BindObjectAsync({ NotifyIfAlreadyBound: true, IgnoreCache: true }, "boundAsync2").then(function (result)
        {
            boundAsync2.hello('CefSharp').then(function (res)
            {
                assert.equal(res, "Hello CefSharp")

                assert.equal(true, CefSharp.DeleteBoundObject("boundAsync2"), "Object was unbound");

                assert.ok(window.boundAsync2 === undefined, "boundAsync2 is now undefined");

                asyncCallback();
            });
        });
    });

})();
