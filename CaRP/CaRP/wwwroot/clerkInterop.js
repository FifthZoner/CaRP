// wwwroot/js/clerkInterop.js
window.clerkInterop = {
    init: async (publishableKey) => {
        if (!window.Clerk) {
            console.error("Clerk script not loaded. Check index.html/App.razor");
            return;
        }
        await Clerk.load();
        console.log("Clerk initialized");
    },

    openSignIn: async () => {
        Clerk.openSignIn();
    },

    getAccessToken: async function () {
            if (!window.Clerk || !window.Clerk.session) {
                return null;
            }
            // This retrieves the short-lived JWT (session token)
            return await window.Clerk.session.getToken();
        },

    logout: async () => {
        await Clerk.signOut();
    }
};