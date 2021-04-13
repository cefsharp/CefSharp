QUnit.module('BindingTestAsync', (hooks) =>
{
    hooks.before(async () =>
    {
        await CefSharp.BindObjectAsync("boundAsync");
    });

    QUnit.test("BindObjectAsync Second call with boundAsync param", async (assert) =>
    {
        const res = await CefSharp.BindObjectAsync("boundAsync");
        assert.equal(res.Success, false, "Second call to BindObjectAsync with already bound objects as params returned false.");
    });

    QUnit.test("Async call (Throw .Net Exception)", async (assert) =>
    {
        assert.rejects(boundAsync.error());
    });

    QUnit.test("Async call (Divide 16 / 2):", async (assert) =>
    {
        const actualResult = await boundAsync.div(16, 2);
        const expectedResult = 8;
        assert.equal(expectedResult, actualResult, "Divide 16 / 2");
    });

    QUnit.test("Async call (Div with Blocking Task 16 / 2):", async (assert) =>
    {
        const actualResult = await boundAsync.divWithBlockingTaskCall(16, 2);
        const expectedResult = 8;
        assert.equal(expectedResult, actualResult, "Divide 16 / 2");
    });

    QUnit.test("Async call (Divide 16 /0)", async (assert) =>
    {
        assert.rejects(boundAsync.div(16, 0));
    });

    QUnit.test("Async call (UIntAddModel 3 + 2):", async (assert) =>
    {
        const actualResult = await boundAsync.uIntAddModel({ ParamA: 3, ParamB: 2 });
        const expectedResult = 5;

        assert.equal(expectedResult, actualResult, "Add 3 + 2 resulted in " + expectedResult);
    });

    QUnit.test("Async call (UIntAdd 3 + 2):", async (assert) =>
    {
        const actualResult = await boundAsync.uIntAdd(3, 2);
        const expectedResult = 5;

        assert.equal(expectedResult, actualResult, "Add 3 + 2 resulted in " + expectedResult);
    });

    QUnit.test("Async call (Hello):", async (assert) =>
    {
        const res = await boundAsync.hello('CefSharp');
        assert.equal(res, "Hello CefSharp")
    });

    QUnit.test("Async call (Long Running Task):", async (assert) =>
    {
        const res = await boundAsync.doSomething();
        assert.ok(true, "Slept for 1000ms")
    });

    QUnit.test("Async call (return Struct):", async (assert) =>
    {
        var res = await boundAsync.returnObject('CefSharp Struct Test');
        assert.equal(res.Value, "CefSharp Struct Test", "Struct with a single field");

    });

    QUnit.test("Async call (return Class):", async (assert) =>
    {
        //Returns a class
        const res = await boundAsync.returnClass('CefSharp Class Test');
        const expectedResult = 'CefSharp Class Test';

        assert.equal(expectedResult, res.Value, "Class with a single property");
    });

    QUnit.test("Async call (return Class as JsonString):", async (assert) =>
    {
        const expectedResult = 'CefSharp Class Test';

        //Returns a class
        const res = await boundAsync.returnClassAsJsonString(expectedResult);
        assert.equal(expectedResult, res.Value, "Class with a single property");
    });

    QUnit.test("Async call (returnStructArray):", async (assert) =>
    {
        const res = await boundAsync.returnStructArray('CefSharp');
        assert.equal(res[0].Value, "CefSharpItem1", "Expected Result of CefSharpItem1");
        assert.equal(res[1].Value, "CefSharpItem2", "Expected Result of CefSharpItem2");
    });

    QUnit.test("Async call (returnClassesArray):", async (assert) =>
    {
        var asyncCallback = assert.async();

        boundAsync.returnClassesArray('CefSharp').then(function (res)
        {
            assert.equal(res[0].Value, "CefSharpItem1", "Expected Result of CefSharpItem1");
            assert.equal(res[1].Value, "CefSharpItem2", "Expected Result of CefSharpItem2");

            asyncCallback();
        });
    });

    QUnit.test("Async call (echoArray):", async (assert) =>
    {
        const res = await boundAsync.echoArray(["one", null, "three"]);

        assert.equal(res.length, 3, "Expected result to be length 3");
        assert.equal(res[0], "one", "Expected Result of 1st item");
        assert.equal(res[1], null, "Expected Result of 2nd item");
        assert.equal(res[2], "three", "Expected Result of 3rd item");
    });

    QUnit.test("Async call (Test Empty Array):", async (assert) =>
    {
        const res = await boundAsync.echoArray([]);

        assert.equal(Array.isArray(res), true, "Expected result is an array");
        assert.equal(res.length, 0, "Expected result to be length 0 (empty array)");
    });


    QUnit.test("Async call (echoValueTypeArray):", async (assert) =>
    {
        const res = await boundAsync.echoValueTypeArray([1, null, 3]);

        assert.equal(res.length, 3, "Expected result to be length 3");
        assert.equal(res[0], 1, "Expected Result of 1st item");
        assert.equal(res[1], 0, "Expected Result of 2nd item");
        assert.equal(res[2], 3, "Expected Result of 3rd item");
    });

    QUnit.test("Async call (echoMultidimensionalArray):", async (assert) =>
    {
        const res = await boundAsync.echoMultidimensionalArray([[1, 2], null, [3, 4]]);

        assert.equal(res.length, 3, "Expected result to be length 3");
        assert.equal(res[0][0], 1, "Expected Result of 1st item");
        assert.equal(res[0][1], 2, "Expected Result of 1st item");
        assert.equal(res[1], null, "Expected Result of 2nd item");
        assert.equal(res[2][0], 3, "Expected Result of 3rd item");
        assert.equal(res[2][1], 4, "Expected Result of 3rd item");
    });

    QUnit.test("Async call (methodReturnList):", async (assert) =>
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

        const results = await Promise.all([
            boundAsync.methodReturnsList(),
            boundAsync.methodReturnsListOfLists(),
        ]);

        assert.deepEqual(results[0], list1, "Call to boundAsync.MethodReturnsList() resulted in : " + JSON.stringify(results[0]));
        assert.deepEqual(results[1], list2, "Call to boundAsync.MethodReturnsListOfLists() resulted in : " + JSON.stringify(results[1]));
    });

    QUnit.test("Async call (methodReturnsDictionary):", async (assert) =>
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

        const results = await Promise.all([
            boundAsync.methodReturnsDictionary1(),
            boundAsync.methodReturnsDictionary2(),
            boundAsync.methodReturnsDictionary3()
        ]);

        assert.deepEqual(results[0], dict1, "Call to boundAsync.MethodReturnsDictionary1() resulted in : " + JSON.stringify(results[0]));
        assert.deepEqual(results[1], dict2, "Call to boundAsync.MethodReturnsDictionary2() resulted in : " + JSON.stringify(results[1]));
        assert.deepEqual(results[2], dict3, "Call to boundAsync.MethodReturnsDictionary3() resulted in : " + JSON.stringify(results[2]));
    });

    QUnit.test("Async call (PassSimpleClassAsArgument):", async (assert) =>
    {
        const res = await boundAsync.passSimpleClassAsArgument({
            TestString: "Hello",
            SubClasses:
                [
                    { PropertyOne: "Test Property One", Numbers: [1, 2, 3] },
                    { PropertyOne: "Test Property Two", Numbers: [4, 5, 6] }
                ]
        });

        assert.equal(res.Item1, true)
        assert.equal(res.Item2, "TestString:Hello;SubClasses[0].PropertyOne:Test Property One");
    });

    QUnit.test("Bind boundAsync2 and call (Hello):", async (assert) =>
    {
        //Can use both cefSharp.bindObjectAsync and CefSharp.BindObjectAsync, both do the same
        const result = await cefSharp.bindObjectAsync({ NotifyIfAlreadyBound: true }, "boundAsync2");
        const res = await boundAsync2.hello('CefSharp');

        assert.equal(res, "Hello CefSharp")
        assert.equal(true, CefSharp.DeleteBoundObject("boundAsync2"), "Object was unbound");
        assert.ok(window.boundAsync2 === undefined, "boundAsync2 is now undefined");
    });

    //Repeat of the previous test to make sure binding a deleted object is working
    QUnit.test("Bind boundAsync2 and call (Hello) Second Attempt:", async (assert) =>
    {
        const result = await CefSharp.BindObjectAsync({ NotifyIfAlreadyBound: true, IgnoreCache: true }, "boundAsync2");
        const res = await boundAsync2.hello('CefSharp');

        assert.equal(res, "Hello CefSharp")
    });

    //Repeat of the previous test to make sure binding a deleted object is working
    QUnit.test("Bind boundAsync2 and call (Hello) Third Attempt:", async (assert) =>
    {
        const result = await CefSharp.BindObjectAsync({ NotifyIfAlreadyBound: true, IgnoreCache: true }, "boundAsync2");
        const res = await boundAsync2.hello('CefSharp');

        assert.equal(res, "Hello CefSharp")
        assert.equal(true, CefSharp.DeleteBoundObject("boundAsync2"), "Object was unbound");
        assert.ok(window.boundAsync2 === undefined, "boundAsync2 is now undefined");
    });

});
