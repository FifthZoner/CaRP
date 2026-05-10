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

    getAccessToken: async () => {
        // This token is what you send to your .NET API
        return await Clerk.session?.getToken();
    },

    logout: async () => {
        await Clerk.signOut();
    }
};