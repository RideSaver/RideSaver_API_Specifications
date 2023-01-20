// Or from '@reduxjs/toolkit/query' if not using the auto-generated hooks
import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";

// initialize an empty api service that we'll inject endpoints into later as needed
export const emptySplitApi = createApi({
    baseQuery: fetchBaseQuery({
        baseUrl: "/api/v1/",
        prepareHeaders(headers, api) {
            const token = (api.getState() as any).auth.token;
            // If we have a token set in state, let's assume that we should be passing it.
            if (token && !headers.has("authorization")) {
                headers.set("authorization", `Bearer ${token}`);
            }
            return headers;
        },
        paramsSerializer: serializeParameters
    }),
    endpoints: () => ({}),
});

function serializeParameters(param: any): string {
    if (typeof param === "object") {
        if (Array.isArray(param)) {
            return param.map(serializeParameters).map(param => `[]=${param}`).join("&")
        } else {
            const newParams = new URLSearchParams();
            Object.entries(param).map(([name, value]) => {
                if(typeof value === "object") {
                    let partParams = new URLSearchParams(serializeParameters(value));
                    for(const [partName, partValue] of partParams) {
                        newParams.set(`${name}.${partName}`, partValue);
                    }
                } else {
                    newParams.set(name, (value as string | number).toString());
                }
            });
            return newParams.toString();
        }
    } else
        return param.toString();
}
