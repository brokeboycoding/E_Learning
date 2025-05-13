$(document).ready(function () {
    $("#mark-complete-btn").click(function (e) {
        e.preventDefault();
        const lessonId = $(this).data("lesson-id");
        const url = $(this).data("url"); // <-- lấy từ data-url

        $.ajax({
            url: url,
            type: 'POST',
            data: { lessonId: lessonId },
            success: function () {
                $("#complete-msg").removeClass("d-none").hide().fadeIn();
                $("#mark-complete-btn").prop("disabled", true).text("✓ Đã hoàn thành");
                $("li:has(a[href*='lessonId=" + lessonId + "'])")
                    .append('<span class="badge bg-success ms-2">✓</span>');
            },
            error: function () {
                alert("Có lỗi xảy ra khi đánh dấu hoàn thành.");
            }
        });
    });
});
