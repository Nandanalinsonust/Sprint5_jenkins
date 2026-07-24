// ✅ GLOBAL CHART REFS
window.barChart = null;
window.pieChart = null;

// ✅ DEPARTMENT BAR CHART
window.renderDepartmentBarChart = (labels, data) => {
    const ctx = document.getElementById("barChart");

    if (!ctx || labels.length === 0) return;

    if (window.barChart) {
        window.barChart.destroy();
    }

    window.barChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Appointments by Department',
                data: data,
                backgroundColor: '#60a5fa'
            }]
        },
        options: {
            responsive: true
        }
    });
};

// ✅ PIE CHART (DOCTOR INSIDE DEPARTMENT)
window.renderPieChart = (labels, data) => {
    const ctx = document.getElementById("pieChart");

    if (!ctx || labels.length === 0) return;

    if (window.pieChart) {
        window.pieChart.destroy();
    }

    window.pieChart = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: labels,
            datasets: [{
                data: data
            }]
        },
        options: {
            responsive: true
        }
    });
};
