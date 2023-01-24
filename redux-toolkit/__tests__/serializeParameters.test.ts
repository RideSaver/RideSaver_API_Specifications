import {test, expect, describe} from "@jest/globals";

import serilizeParameters from "../serializeParameters";

describe("serializeParams", () => {
    test("Correctly serializes primatives", ()=>{
        expect(serilizeParameters(5)).toBe("5");
        expect(serilizeParameters(5n)).toBe("5");
        expect(serilizeParameters("a")).toBe("a");
        expect(serilizeParameters(undefined)).toBe("");
    });
    test("Correctly serializes objects", ()=>{
        expect(serilizeParameters({})).toBe("");
        expect(serilizeParameters({
            a: "b"
        })).toBe("a=b");
        expect(serilizeParameters({
            a: "b",
            b: 2
        })).toBe("a=b&b=2");
        expect(serilizeParameters({
            a: 2,
            b: undefined
        })).toBe("a=2");
    });
    test("Correctly serializes nested objects", () => {
        expect(serilizeParameters({
            a: {
                b: 2
            }
        })).toBe("a.b=2");
    });

    test("Correctly serializes arrays", () => {
        expect(serilizeParameters([1, 2, 3])).toBe("[0]=1&[1]=2&[2]=3");
    });

    test("Correctly serializes arrays in objects", () => {
        expect(serilizeParameters({
            a: [1, 2, 3]
        })).toBe("a[0]=1&a[1]=2&a[2]=3");
    });
});