import { Api } from "./Api";

export const getCurrentUserId = () => localStorage.getItem("userId") ?? "1";

// If Bun inlines the variable this is a plain string; if not, the ReferenceError is caught.
let envUrl: string | undefined;
try {
    envUrl = process.env.BUN_PUBLIC_API_URL;
} catch {
    envUrl = undefined;
}

// Fallback: same host the page was loaded from, API port
const baseUrl = envUrl || `${location.protocol}//${location.hostname}:5034`;

export const MyApi = new Api({
    baseUrl,
    customFetch: (input, init) =>
        fetch(input, {
            ...init,
            headers: { ...(init?.headers ?? {}), "X-User-Id": getCurrentUserId() },
        }),
});