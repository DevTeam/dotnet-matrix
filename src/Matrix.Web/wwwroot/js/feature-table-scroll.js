(() => {
    const phone = window.matchMedia('(max-width: 680px)');
    const dock = document.createElement('div');
    const track = document.createElement('div');
    const thumb = document.createElement('button');
    dock.className = 'feature-scroll-dock';
    track.className = 'feature-scroll-dock-track';
    thumb.className = 'feature-scroll-dock-thumb';
    thumb.type = 'button';
    thumb.setAttribute('role', 'scrollbar');
    thumb.setAttribute('aria-label', 'Scroll feature libraries horizontally');
    thumb.setAttribute('aria-orientation', 'horizontal');
    thumb.setAttribute('aria-valuemin', '0');
    track.append(thumb);
    dock.append(track);
    document.body.append(dock);

    let active;
    let frame;
    let dragStartX;
    let dragStartScroll;
    const observed = new WeakSet();
    const resizeObserver = new ResizeObserver(scheduleUpdate);

    const featureTables = () => [...document.querySelectorAll('.table-scroll')]
        .filter(scroller => scroller.querySelector('.feature-table'));

    const bindTables = () => {
        for (const scroller of featureTables()) {
            if (observed.has(scroller)) {
                continue;
            }

            observed.add(scroller);
            resizeObserver.observe(scroller);
            resizeObserver.observe(scroller.querySelector('.feature-table'));
            scroller.addEventListener('scroll', () => {
                if (scroller === active) {
                    updateThumb();
                }
            }, { passive: true });
        }
    };

    const updateThumb = () => {
        if (!active) {
            return;
        }

        const maximum = Math.max(0, active.scrollWidth - active.clientWidth);
        const trackWidth = track.clientWidth;
        const thumbWidth = Math.max(44, trackWidth * active.clientWidth / active.scrollWidth);
        const travel = Math.max(0, trackWidth - thumbWidth);
        const left = maximum === 0 ? 0 : active.scrollLeft / maximum * travel;
        thumb.style.width = `${thumbWidth}px`;
        thumb.style.transform = `translateX(${left}px)`;
        thumb.setAttribute('aria-valuemax', String(Math.round(maximum)));
        thumb.setAttribute('aria-valuenow', String(Math.round(active.scrollLeft)));
    };

    const update = () => {
        frame = undefined;
        bindTables();

        if (!phone.matches) {
            active = undefined;
            dock.classList.remove('is-visible');
            return;
        }

        active = featureTables().find(scroller => {
            const bounds = scroller.getBoundingClientRect();
            return bounds.bottom > 0
                && bounds.top < window.innerHeight
                && scroller.scrollWidth > scroller.clientWidth + 1;
        });
        if (!active) {
            dock.classList.remove('is-visible');
            return;
        }

        const bounds = active.getBoundingClientRect();
        const left = Math.max(8, bounds.left);
        const right = Math.min(window.innerWidth - 8, bounds.right);
        dock.style.left = `${left}px`;
        dock.style.width = `${Math.max(0, right - left)}px`;
        dock.classList.add('is-visible');
        updateThumb();
    };

    function scheduleUpdate() {
        if (frame !== undefined) {
            return;
        }

        frame = window.requestAnimationFrame(update);
    }

    const scrollFromPointer = clientX => {
        if (!active) {
            return;
        }

        const maximum = Math.max(0, active.scrollWidth - active.clientWidth);
        const travel = Math.max(1, track.clientWidth - thumb.offsetWidth);
        active.scrollLeft = dragStartScroll + (clientX - dragStartX) / travel * maximum;
    };

    thumb.addEventListener('pointerdown', event => {
        if (!active) {
            return;
        }

        dragStartX = event.clientX;
        dragStartScroll = active.scrollLeft;
        thumb.setPointerCapture(event.pointerId);
        event.preventDefault();
    });
    thumb.addEventListener('pointermove', event => {
        if (thumb.hasPointerCapture(event.pointerId)) {
            scrollFromPointer(event.clientX);
        }
    });
    track.addEventListener('pointerdown', event => {
        if (!active || event.target === thumb) {
            return;
        }

        const bounds = track.getBoundingClientRect();
        const maximum = Math.max(0, active.scrollWidth - active.clientWidth);
        const travel = Math.max(1, track.clientWidth - thumb.offsetWidth);
        active.scrollLeft = (event.clientX - bounds.left - thumb.offsetWidth / 2) / travel * maximum;
        event.preventDefault();
    });
    thumb.addEventListener('keydown', event => {
        if (!active) {
            return;
        }

        const step = Math.max(56, active.clientWidth * .8);
        const movements = {
            ArrowLeft: -56,
            ArrowRight: 56,
            PageUp: -step,
            PageDown: step
        };
        if (event.key === 'Home') {
            active.scrollLeft = 0;
        } else if (event.key === 'End') {
            active.scrollLeft = active.scrollWidth;
        } else if (movements[event.key] !== undefined) {
            active.scrollLeft += movements[event.key];
        } else {
            return;
        }

        event.preventDefault();
    });

    window.addEventListener('scroll', scheduleUpdate, { passive: true });
    window.addEventListener('resize', scheduleUpdate, { passive: true });
    phone.addEventListener('change', scheduleUpdate);
    new MutationObserver(scheduleUpdate).observe(
        document.getElementById('app'),
        { childList: true, subtree: true });
    scheduleUpdate();
})();
