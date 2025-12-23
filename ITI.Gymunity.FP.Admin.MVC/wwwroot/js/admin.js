/* ============================================
   Gymunity Admin Dashboard - JavaScript
   ============================================ */

document.addEventListener('DOMContentLoaded', function () {
    // Initialize Components
    initSidebarToggle();
    initCurrentTime();
    initTooltips();
    initPopovers();
    setActiveNavLink();
    initDarkMode();
});

/* ============================================
   SIDEBAR TOGGLE
   ============================================ */

function initSidebarToggle() {
    const sidebarToggle = document.getElementById('sidebarToggle');
    const sidebar = document.getElementById('sidebar');
    const mainContent = document.querySelector('.main-content');
    const pageContainer = document.querySelector('.page-container');

    if (!sidebarToggle || !sidebar) return;

    // Check localStorage for sidebar state
    const sidebarCollapsed = localStorage.getItem('sidebarCollapsed') === 'true';
    if (sidebarCollapsed && window.innerWidth > 768) {
        sidebar.classList.add('collapsed');
        mainContent.classList.add('sidebar-collapsed');
    }

    // Desktop toggle
    sidebarToggle.addEventListener('click', function (e) {
        e.preventDefault();
        if (window.innerWidth > 768) {
            sidebar.classList.toggle('collapsed');
            mainContent.classList.toggle('sidebar-collapsed');
            localStorage.setItem('sidebarCollapsed', sidebar.classList.contains('collapsed'));
        } else {
            // Mobile: toggle show/hide
            sidebar.classList.toggle('show');
            pageContainer.style.position = sidebar.classList.contains('show') ? 'fixed' : 'relative';
        }
    });

    // Mobile: Close sidebar when clicking a link
    const navLinks = document.querySelectorAll('.sidebar-item .nav-link');
    navLinks.forEach(link => {
        link.addEventListener('click', function () {
            if (window.innerWidth <= 768) {
                sidebar.classList.remove('show');
                pageContainer.style.position = 'relative';
            }
        });
    });

    // Close sidebar when clicking outside on mobile
    document.addEventListener('click', function (e) {
        if (window.innerWidth <= 768 && sidebar.classList.contains('show')) {
            if (!sidebar.contains(e.target) && !sidebarToggle.contains(e.target)) {
                sidebar.classList.remove('show');
                pageContainer.style.position = 'relative';
            }
        }
    });

    // Handle window resize
    window.addEventListener('resize', function () {
        if (window.innerWidth > 768) {
            sidebar.classList.remove('show');
            pageContainer.style.position = 'relative';
        }
    });
}

/* ============================================
   ACTIVE NAV LINK
   ============================================ */

function setActiveNavLink() {
    const currentUrl = window.location.pathname.toLowerCase();
    const navLinks = document.querySelectorAll('.sidebar-item .nav-link');

    navLinks.forEach(link => {
        const href = link.getAttribute('href').toLowerCase();
        if (currentUrl.includes(href) && href !== '/') {
            link.classList.add('active');
            link.setAttribute('aria-current', 'page');
        } else {
            link.classList.remove('active');
            link.removeAttribute('aria-current');
        }
    });
}

/* ============================================
   CURRENT TIME
   ============================================ */

function initCurrentTime() {
    const timeElement = document.getElementById('currentTime');
    if (!timeElement) return;

    function updateTime() {
        const now = new Date();
        const options = {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit',
            hour12: true
        };
        timeElement.textContent = now.toLocaleDateString('en-US', options);
    }

    updateTime();
    setInterval(updateTime, 60000); // Update every minute
}

/* ============================================
   BOOTSTRAP TOOLTIPS
   ============================================ */

function initTooltips() {
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

/* ============================================
   BOOTSTRAP POPOVERS
   ============================================ */

function initPopovers() {
    const popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });
}

/* ============================================
   DARK MODE TOGGLE
   ============================================ */

function initDarkMode() {
    const darkModeToggle = document.getElementById('darkModeToggle');
    if (!darkModeToggle) return;

    // Check for saved preference or default to light mode
    const currentTheme = localStorage.getItem('theme') || 'light';
    applyTheme(currentTheme);

    darkModeToggle.addEventListener('click', function () {
        const newTheme = document.documentElement.getAttribute('data-theme') === 'dark' ? 'light' : 'dark';
        applyTheme(newTheme);
        localStorage.setItem('theme', newTheme);
    });
}

function applyTheme(theme) {
    if (theme === 'dark') {
        document.documentElement.setAttribute('data-theme', 'dark');
        document.body.classList.add('dark-mode');
    } else {
        document.documentElement.setAttribute('data-theme', 'light');
        document.body.classList.remove('dark-mode');
    }
}

/* ============================================
   ALERT AUTO-DISMISS
   ============================================ */

document.addEventListener('DOMContentLoaded', function () {
    const alerts = document.querySelectorAll('.alert:not(.alert-permanent)');
    alerts.forEach(alert => {
        setTimeout(() => {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000); // Auto-dismiss after 5 seconds
    });
});

/* ============================================
   FORM VALIDATION
   ============================================ */

function validateForm(formId) {
    const form = document.getElementById(formId);
    if (!form) return false;

    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
    }

    form.classList.add('was-validated');
    return form.checkValidity();
}

/* ============================================
   LOADING SPINNER
   ============================================ */

function showLoadingSpinner(buttonId = null) {
    if (buttonId) {
        const button = document.getElementById(buttonId);
        if (button) {
            const originalContent = button.innerHTML;
            button.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Loading...';
            button.disabled = true;
            button.dataset.originalContent = originalContent;
        }
    }
}

function hideLoadingSpinner(buttonId = null) {
    if (buttonId) {
        const button = document.getElementById(buttonId);
        if (button) {
            button.innerHTML = button.dataset.originalContent || 'Submit';
            button.disabled = false;
        }
    }
}

/* ============================================
   CONFIRMATION DIALOG
   ============================================ */

function confirmAction(message = 'Are you sure?') {
    return new Promise((resolve) => {
        if (confirm(message)) {
            resolve(true);
        } else {
            resolve(false);
        }
    });
}

/* ============================================
   COPY TO CLIPBOARD
   ============================================ */

function copyToClipboard(text, feedbackId = null) {
    navigator.clipboard.writeText(text).then(() => {
        if (feedbackId) {
            const element = document.getElementById(feedbackId);
            if (element) {
                const originalText = element.textContent;
                element.textContent = 'Copied!';
                setTimeout(() => {
                    element.textContent = originalText;
                }, 2000);
            }
        }
        showNotification('Copied to clipboard!', 'success');
    }).catch(err => {
        showNotification('Failed to copy', 'error');
    });
}

/* ============================================
   NOTIFICATIONS
   ============================================ */

function showNotification(message, type = 'info', duration = 3000) {
    const alertClass = `alert-${type}`;
    const alertHTML = `
        <div class="alert alert-${type} alert-dismissible fade show" role="alert" style="position: fixed; bottom: 20px; right: 20px; z-index: 1050; max-width: 400px;">
            <i class="fas fa-${getIconForType(type)} me-2"></i>
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `;

    const container = document.createElement('div');
    container.innerHTML = alertHTML;
    document.body.appendChild(container);

    const alert = container.querySelector('.alert');
    const bsAlert = new bootstrap.Alert(alert);

    setTimeout(() => {
        bsAlert.close();
    }, duration);
}

function getIconForType(type) {
    const icons = {
        'success': 'check-circle',
        'danger': 'exclamation-circle',
        'warning': 'exclamation-triangle',
        'info': 'info-circle'
    };
    return icons[type] || 'info-circle';
}

/* ============================================
   TABLE ROW ACTIONS
   ============================================ */

function setupTableRowActions() {
    const rows = document.querySelectorAll('tbody tr');

    rows.forEach(row => {
        row.addEventListener('mouseenter', function () {
            const actionButtons = this.querySelectorAll('.action-btn');
            actionButtons.forEach(btn => {
                btn.style.opacity = '1';
            });
        });

        row.addEventListener('mouseleave', function () {
            const actionButtons = this.querySelectorAll('.action-btn');
            actionButtons.forEach(btn => {
                btn.style.opacity = '0.7';
            });
        });
    });
}

/* ============================================
   DATE FORMATTING
   ============================================ */

function formatDate(dateString, format = 'short') {
    const date = new Date(dateString);
    const options = format === 'short'
        ? { year: 'numeric', month: 'short', day: 'numeric' }
        : { year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit' };

    return date.toLocaleDateString('en-US', options);
}

/* ============================================
   CURRENCY FORMATTING
   ============================================ */

function formatCurrency(amount, currency = 'EGP') {
    return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: currency,
    }).format(amount);
}

/* ============================================
   NUMBER FORMATTING
   ============================================ */

function formatNumber(number) {
    return new Intl.NumberFormat('en-US').format(number);
}

/* ============================================
   SEARCH & FILTER
   ============================================ */

function initSearchFilter(inputId, tableId) {
    const searchInput = document.getElementById(inputId);
    const table = document.getElementById(tableId);

    if (!searchInput || !table) return;

    searchInput.addEventListener('keyup', function () {
        const filter = this.value.toUpperCase();
        const rows = table.querySelectorAll('tbody tr');

        rows.forEach(row => {
            const text = row.textContent.toUpperCase();
            row.style.display = text.includes(filter) ? '' : 'none';
        });
    });
}

/* ============================================
   EXPORT TABLE TO CSV
   ============================================ */

function exportTableToCSV(tableId, filename = 'export.csv') {
    const table = document.getElementById(tableId);
    if (!table) return;

    let csv = [];
    const rows = table.querySelectorAll('tr');

    rows.forEach(row => {
        const rowData = [];
        row.querySelectorAll('th, td').forEach(cell => {
            rowData.push('"' + cell.textContent.trim().replace(/"/g, '""') + '"');
        });
        csv.push(rowData.join(','));
    });

    const csvContent = csv.join('\n');
    const blob = new Blob([csvContent], { type: 'text/csv' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    link.click();
    window.URL.revokeObjectURL(url);
}

/* ============================================
   PRINT PAGE
   ============================================ */

function printPage() {
    window.print();
}

/* ============================================
   PAGE UTILITIES
   ============================================ */

// Scroll to top
function scrollToTop() {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
}

// Create scroll to top button
document.addEventListener('DOMContentLoaded', function () {
    // Show/hide scroll to top button
    const scrollToTopBtn = document.createElement('button');
    scrollToTopBtn.id = 'scrollToTopBtn';
    scrollToTopBtn.className = 'btn btn-primary btn-floating';
    scrollToTopBtn.innerHTML = '<i class="fas fa-arrow-up"></i>';
    scrollToTopBtn.style.cssText = `
        position: fixed;
        bottom: 20px;
        right: 20px;
        display: none;
        width: 50px;
        height: 50px;
        padding: 0;
        border-radius: 50%;
        z-index: 999;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
    `;
    document.body.appendChild(scrollToTopBtn);

    window.addEventListener('scroll', function () {
        if (window.scrollY > 300) {
            scrollToTopBtn.style.display = 'flex';
            scrollToTopBtn.style.alignItems = 'center';
            scrollToTopBtn.style.justifyContent = 'center';
        } else {
            scrollToTopBtn.style.display = 'none';
        }
    });

    scrollToTopBtn.addEventListener('click', scrollToTop);
});

/* ============================================
   API HELPERS
   ============================================ */

async function fetchWithAuth(url, options = {}) {
    const defaultOptions = {
        headers: {
            'Content-Type': 'application/json',
        }
    };

    const finalOptions = { ...defaultOptions, ...options };

    try {
        const response = await fetch(url, finalOptions);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        return await response.json();
    } catch (error) {
        console.error('Fetch error:', error);
        showNotification('An error occurred. Please try again.', 'danger');
        throw error;
    }
}

/* ============================================
   DEBUG MODE
   ============================================ */

window.adminDebug = {
    enableDebugMode: function () {
        localStorage.setItem('debugMode', 'true');
        console.log('Debug mode enabled');
    },
    disableDebugMode: function () {
        localStorage.removeItem('debugMode');
        console.log('Debug mode disabled');
    },
    isDebugMode: function () {
        return localStorage.getItem('debugMode') === 'true';
    },
    log: function (message, data) {
        if (this.isDebugMode()) {
            console.log(`[ADMIN DEBUG] ${message}`, data);
        }
    }
};

// Expose utilities to window
window.adminUtils = {
    showLoadingSpinner,
    hideLoadingSpinner,
    confirmAction,
    copyToClipboard,
    showNotification,
    formatDate,
    formatCurrency,
    formatNumber,
    validateForm,
    exportTableToCSV,
    printPage,
    scrollToTop,
    fetchWithAuth
};
