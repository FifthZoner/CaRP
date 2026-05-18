// wwwroot/js/clerkInterop.js
window.clerkInterop = {
    initPromise: null,

    init: async function () {
        if (!window.Clerk) {
            console.error("Clerk script not loaded yet.");
            return;
        }
        if (!this.initPromise) {
            this.initPromise = window.Clerk.load();
        }
        await this.initPromise;
        console.log("Clerk initialized successfully");
    },

    openSignIn: async () => {
        Clerk.openSignIn();
    },

    getAccessToken: async function () {
        await this.init();
        
        if (!window.Clerk || !window.Clerk.session) {
            return null;
        }
            // This retrieves the short-lived JWT (session token)
            return await window.Clerk.session.getToken({ template: "rolelevel" });
        },

    logout: async () => {
        await Clerk.signOut();
    }
};

window.clerkInterop.init();