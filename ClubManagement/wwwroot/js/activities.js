// Activities Management JavaScript for Razor Pages
// Handles search, filter, and dynamic interactions

document.addEventListener('DOMContentLoaded', function() {
    // Initialize all features
    initializeSearch();
    initializeFilters();
    initializeAnimations();
    initializeClearButton();
});

// ========== Search Functionality ==========
function initializeSearch() {
    const searchInput = document.getElementById('searchInput');
    if (!searchInput) return;

    searchInput.addEventListener('input', debounce(function(e) {
        filterActivities();
    }, 300));
}

// ========== Filter Functionality ==========
function initializeFilters() {
    const clubFilter = document.getElementById('clubFilter');
    const statusFilter = document.getElementById('statusFilter');

    if (clubFilter) {
        clubFilter.addEventListener('change', filterActivities);
    }

    if (statusFilter) {
        statusFilter.addEventListener('change', filterActivities);
    }
}

function initializeClearButton() {
    const clearButton = document.getElementById('clearFilters');
    if (!clearButton) return;

    clearButton.addEventListener('click', function() {
        // Clear all inputs and filters
        const searchInput = document.getElementById('searchInput');
        const clubFilter = document.getElementById('clubFilter');
        const statusFilter = document.getElementById('statusFilter');

        if (searchInput) searchInput.value = '';
        if (clubFilter) clubFilter.value = '';
        if (statusFilter) statusFilter.value = '';

        // Re-filter to show all
        filterActivities();
    });
}

function filterActivities() {
    const searchTerm = document.getElementById('searchInput')?.value.toLowerCase() || '';
    const selectedClub = document.getElementById('clubFilter')?.value || '';
    const selectedStatus = document.getElementById('statusFilter')?.value || '';

    const activityCards = document.querySelectorAll('.activity-card');
    const noResultsDiv = document.getElementById('noResults');
    let visibleCount = 0;

    activityCards.forEach(card => {
        const activityName = card.querySelector('.card-title')?.textContent.toLowerCase() || '';
        const location = card.querySelector('.card-text')?.textContent.toLowerCase() || '';
        const clubId = card.dataset.clubId || '';
        const status = card.dataset.status || '';

        const matchesSearch = activityName.includes(searchTerm) || location.includes(searchTerm);
        const matchesClub = !selectedClub || clubId === selectedClub;
        const matchesStatus = !selectedStatus || status === selectedStatus;

        if (matchesSearch && matchesClub && matchesStatus) {
            card.style.display = '';
            card.classList.add('fade-in');
            visibleCount++;
        } else {
            card.style.display = 'none';
            card.classList.remove('fade-in');
        }
    });

    // Show/hide "no results" message
    if (noResultsDiv) {
        if (visibleCount === 0) {
            noResultsDiv.classList.remove('d-none');
        } else {
            noResultsDiv.classList.add('d-none');
        }
    }
}

// ========== Animations ==========
function initializeAnimations() {
    // Hover effect for activity cards
    const activityCards = document.querySelectorAll('.hover-lift');
    
    activityCards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-5px)';
        });

        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0)';
        });
    });

    // Fade in animation on page load
    observeElements();
}

function observeElements() {
    const observer = new IntersectionObserver(entries => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('fade-in');
            }
        });
    }, { threshold: 0.1 });

    document.querySelectorAll('.activity-card').forEach(card => {
        observer.observe(card);
    });
}

// ========== Utility Functions ==========
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// ========== CSS Animations ==========
const style = document.createElement('style');
style.textContent = `
    .fade-in {
        animation: fadeIn 0.5s ease-in;
    }

    @keyframes fadeIn {
        from {
            opacity: 0;
            transform: translateY(20px);
        }
        to {
            opacity: 1;
            transform: translateY(0);
        }
    }

    .hover-lift {
        transition: all 0.3s ease;
    }

    .hover-lift:hover {
        box-shadow: 0 10px 30px rgba(0,0,0,0.15) !important;
    }

    .activity-card {
        transition: all 0.3s ease;
    }
`;
document.head.appendChild(style);

// Export functions for global use
window.ActivityManagement = {
    filterActivities
};
