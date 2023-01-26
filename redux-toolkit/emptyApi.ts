// Or from '@reduxjs/toolkit/query' if not using the auto-generated hooks
import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import serializeParameters from "./serializeParameters";

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
