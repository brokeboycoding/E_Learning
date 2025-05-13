$(document).ready(function () {
    const video = document.querySelector("video");
    const lessonId = $("#lessonId").val();

    function formatTime(seconds) {
        const m = Math.floor(seconds / 60);
        const s = Math.floor(seconds % 60);
        return `${m}:${s.toString().padStart(2, '0')}`;
    }

    $(document).off("click", "#save-note").on("click", "#save-note", function () {
        const content = $("#note-content").val();
        if (!content) return;

        const timestamp = video.currentTime;

        $.ajax({
            url: '/Lesson/AddNote',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                lessonId: lessonId,
                content: content,
                timestamp: timestamp
            }),
            success: function () {
                $("#note-content").val('');
                loadNotes();
            }
        });
    });

    function loadNotes() {
        $.get(`/Lesson/GetNotes?lessonId=${lessonId}`, function (data) {
            const list = $("#note-list");
            list.empty();
            data.forEach(n => {
                const formatted = formatTime(n.timestamp);
                list.append(`
                    <li class="list-group-item d-flex justify-content-between align-items-start">
                        <div>
                            <a href="#" onclick="video.currentTime=${n.timestamp}; video.play(); return false;">
                                ▶ Phát từ ${formatted}
                            </a>
                            <p class="mb-1">${n.content}</p>
                        </div>
                        <button class="btn btn-sm btn-danger" onclick="deleteNote(${n.id})">Xóa</button>
                    </li>
                `);
            });
        });
    }

    // Đưa ra global scope
    window.deleteNote = function (id) {
        if (confirm("Bạn có chắc chắn muốn xóa ghi chú này?")) {
            $.post(`/Lesson/DeleteNote?id=${id}`, function () {
                loadNotes();
            });
        }
    };

    loadNotes();
});
