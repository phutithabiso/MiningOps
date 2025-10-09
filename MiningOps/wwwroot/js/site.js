// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', function () {
    // Get input elements
    const itemNameInput = document.getElementById('ItemName');
    const descriptionInput = document.getElementById('Description');
    const quantityInput = document.getElementById('Quantity');
    const reorderLevelInput = document.getElementById('ReorderLevel');
    const unitCostInput = document.getElementById('UnitCost');
    const warehouseSelect = document.getElementById('WarehouseId');
    const lastUpdatedInput = document.getElementById('LastUpdated');
    const unitSelect = document.getElementById('Unit');
    const totalValueDisplay = document.getElementById('totalValue');
    const stockStatusDisplay = document.getElementById('stockStatus');

    // Calculate total value
    function calculateTotalValue() {
        const quantity = parseFloat(quantityInput.value) || 0;
        const unitCost = parseFloat(unitCostInput.value) || 0;
        const totalValue = quantity * unitCost;
        totalValueDisplay.textContent = totalValue.toFixed(2);
    }

    // Update stock status
    function updateStockStatus() {
        const quantity = parseFloat(quantityInput.value) || 0;
        const reorderLevel = parseFloat(reorderLevelInput.value) || 0;

        let status = 'Unknown';
        let statusClass = 'secondary';

        if (quantity <= 0) {
            status = 'Out of Stock';
            statusClass = 'danger';
        } else if (reorderLevel > 0 && quantity <= reorderLevel) {
            status = 'Low Stock';
            statusClass = 'warning';
        } else if (quantity > 0) {
            status = 'In Stock';
            statusClass = 'success';
        }

        stockStatusDisplay.textContent = status;
        stockStatusDisplay.className = `badge bg-${statusClass}`;
    }

    // Update validation state
    function updateValidationState(input) {
        if (input.value && input.value.trim() !== '') {
            input.classList.add('is-valid');
            input.classList.remove('is-invalid');
        } else if (input.hasAttribute('required')) {
            input.classList.remove('is-valid');
            input.classList.add('is-invalid');
        } else {
            input.classList.remove('is-valid', 'is-invalid');
        }
    }

    // Set current datetime for LastUpdated if empty
    if (lastUpdatedInput && !lastUpdatedInput.value) {
        const now = new Date();
        now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
        lastUpdatedInput.value = now.toISOString().slice(0, 16);
    }

    // Event listeners for real-time updates
    quantityInput.addEventListener('input', function () {
        calculateTotalValue();
        updateStockStatus();
        updateValidationState(this);
    });

    unitCostInput.addEventListener('input', function () {
        calculateTotalValue();
        updateValidationState(this);
    });

    reorderLevelInput.addEventListener('input', function () {
        updateStockStatus();
        updateValidationState(this);
    });

    itemNameInput.addEventListener('blur', function () {
        updateValidationState(this);
    });

    warehouseSelect.addEventListener('change', function () {
        updateValidationState(this);
    });

    // Initialize all fields
    const inputs = [itemNameInput, descriptionInput, quantityInput, reorderLevelInput,
        unitCostInput, warehouseSelect, lastUpdatedInput, unitSelect];

    inputs.forEach(input => {
        if (input) {
            input.addEventListener('blur', function () {
                updateValidationState(this);
            });

            // Set initial validation state
            if (input.value && input.value.trim() !== '') {
                input.classList.add('is-valid');
            } else if (input.hasAttribute('required') && !input.value) {
                input.classList.add('is-invalid');
            }
        }
    });

    // Initialize calculations
    calculateTotalValue();
    updateStockStatus();
});