export default function serializeParameters(param: any): string {
    if (param === undefined) return "";
    if (typeof param === "object") {
        if (Array.isArray(param)) {
            return param.map(serializeParameters).map((param, index) => `[${index}]=${param}`).join("&")
        } else {
            const newParams = new URLSearchParams();
            Object.entries(param).filter(([_, value]) => value !== undefined).map(([name, value]) => {
                const serialized = serializeParameters(value);
                if (typeof value === "object") {
                    let partParams = new URLSearchParams(serialized);
                    for (const [partName, partValue] of partParams) {
                        newParams.set(`${name}.${partName}`, partValue);
                    }
                } else {
                    newParams.set(name, serialized);
                }
            });
            return newParams.toString();
        }
    } else
        return param.toString();
}