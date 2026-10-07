import { Api } from "./Api";

export const getCurrentUserId = () => localStorage.getItem("userId") ?? "1";

export const MyApi = new Api({
    customFetch: (input, init) =>
        fetch(input, {
            ...init,
            headers: { ...(init?.headers ?? {}), "X-User-Id": getCurrentUserId() },
        }),
});