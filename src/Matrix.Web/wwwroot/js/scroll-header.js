(() => {
    const changeDelay = 160;
    let applied = window.scrollY > 0;
    let pending = applied;
    let timer;
    let headerObserver;

    const observeHeader = () => {
        const header = document.querySelector('.topbar');
        if (!header || headerObserver) {
            return Boolean(header);
        }

        const updateHeight = () => {
            document.documentElement.style.setProperty(
                '--topbar-height',
                `${Math.ceil(header.getBoundingClientRect().height)}px`);
        };
        headerObserver = new ResizeObserver(updateHeight);
        headerObserver.observe(header);
        updateHeight();
        return true;
    };

    const apply = () => {
        applied = pending;
        document.documentElement.classList.toggle('page-scrolled', applied);
    };

    const scheduleUpdate = () => {
        const next = window.scrollY > 0;
        if (next === applied) {
            pending = applied;
            window.clearTimeout(timer);
            return;
        }

        if (next === pending) {
            return;
        }

        pending = next;
        window.clearTimeout(timer);
        timer = window.setTimeout(apply, changeDelay);
    };

    window.addEventListener('scroll', scheduleUpdate, { passive: true });
    window.addEventListener('pageshow', () => {
        pending = window.scrollY > 0;
        apply();
    });

    if (!observeHeader()) {
        const app = document.getElementById('app');
        const appObserver = new MutationObserver(() => {
            if (observeHeader()) {
                appObserver.disconnect();
            }
        });
        appObserver.observe(app, { childList: true, subtree: true });
    }

    apply();
})();
