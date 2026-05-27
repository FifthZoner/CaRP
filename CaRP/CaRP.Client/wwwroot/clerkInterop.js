// wwwroot/js/clerkInterop.js
window.clerkInterop = {
    initPromise: null,

    init: async function () {
        // Polling loop: If Clerk script is still downloading, wait for it
        let attempts = 0;
        while (!window.Clerk && attempts < 50) {
            await new Promise(resolve => setTimeout(resolve, 100));
            attempts++;
        }

        if (!window.Clerk) {
            console.error("Clerk script failed to load after 5 seconds.");
            return;
        }

        if (!this.initPromise) {
            this.initPromise = window.Clerk.load();
        }
        await this.initPromise;
    },

    hasActiveSession: async function () {
        await this.init();
        return !!(window.Clerk && window.Clerk.session);
    },

    openSignIn: async function () {
        await this.init();
        window.Clerk.openSignIn();
    },

    openSignUp: async function () {
        await this.init();
        window.Clerk.openSignUp();
    },

    getAccessToken: async function () {
        await this.init();
        if (!window.Clerk || !window.Clerk.session) return null;
        try {
            return await window.Clerk.session.getToken({ template: "rolelevel" });
        } catch (e) {
            return null;
        }
    },

    logout: async function () {
        await this.init();
        if (window.Clerk && window.Clerk.session) {
            await window.Clerk.signOut();
        }
    }
};