$(document).ready(function () {
    const video = document.getElementById('player');

    // 🎯 Tự động gán timestamp khi bắt đầu nhập ghi chú mới
    if (video) {
        $('textarea[name="Content"]').on('focus', function () {
            const currentTime = video.currentTime.toFixed(1);
            $('input[name="Timestamp"]').val(currentTime);
        });
    }

    // ➕ Gửi ghi chú mới bằng AJAX
    $('#noteForm').submit(function (e) {
        e.preventDefault();

        // Lấy thời gian hiện tại của video nếu chưa có
        const timestampInput = $('input[name="Timestamp"]');
        if (!timestampInput.val()) {
            const video = document.getElementById('player');
            if (video) {
                timestampInput.val(video.currentTime.toFixed(1));
            }
        }

        const formData = $(this).serialize();

        $.ajax({
            type: 'POST',
            url: addNoteUrl,
            data: formData,
            success: function (res) {
                if (res.success) {
                    const mmss = new Date(res.note.timestamp * 1000).toISOString().substr(14, 5);

                    $('#note-alert').html('<div class="alert alert-success">Ghi chú đã được thêm!</div>');

                    $('#noteList').prepend(`
                        <li class="list-group-item" id="note-${res.note.id}">
                            <div class="d-flex justify-content-between align-items-start">
                                <div class="me-3">
                                    <div class="fw-bold">
                                        <a href="#" class="text-primary note-jump" data-timestamp="${res.note.timestamp}">
                                            ${res.note.content}
                                        </a>
                                    </div>
                                    <small class="text-muted"><i class="fas fa-clock"></i> ${mmss}</small>
                                </div>
                                <div>
                                    <button class="btn btn-sm btn-outline-secondary edit-note"
                                            data-id="${res.note.id}" data-content="${res.note.content}" data-timestamp="${res.note.timestamp}">
                                        <i class="fas fa-edit"></i>
                                    </button>
                                    <button class="btn btn-sm btn-outline-danger delete-note" data-id="${res.note.id}">
                                        <i class="fas fa-trash"></i>
                                    </button>
                                </div>
                            </div>
                        </li>
                    `);
                    $('#noteForm')[0].reset();
                } else {
                    $('#note-alert').html(`<div class="alert alert-danger">${res.message}</div>`);
                }
            },
            error: function () {
                $('#note-alert').html('<div class="alert alert-danger">Lỗi không xác định.</div>');
            }
        });
    });

    // 🗑️ Xóa ghi chú
    $(document).on('click', '.delete-note', function () {
        const noteId = $(this).data('id');
        $.ajax({
            type: 'POST',
            url: deleteNoteUrl,
            data: { id: noteId },
            success: function (res) {
                if (res.success) {
                    $(`#note-${noteId}`).remove();
                } else {
                    alert(res.message);
                }
            }
        });
    });

    // ⏩ Click nhảy video
    $(document).on('click', '.note-jump', function (e) {
        e.preventDefault();
        const seconds = parseFloat($(this).data('timestamp'));
        if (video) {
            video.currentTime = seconds;
            video.play();
        }
    });

    // ✏️ Mở modal chỉnh sửa ghi chú (tự động gán timestamp hiện tại của video)
    $(document).on('click', '.edit-note', function () {
        $('#editNoteId').val($(this).data('id'));
        $('#editContent').val($(this).data('content'));

        // Gán thời điểm hiện tại trong video
        if (video) {
            $('#editTimestamp').val(video.currentTime.toFixed(1));
        } else {
            $('#editTimestamp').val($(this).data('timestamp'));
        }

        const modal = new bootstrap.Modal(document.getElementById('editNoteModal'));
        modal.show();
    });

    // 💾 Lưu chỉnh sửa ghi chú
    $('#editNoteForm').submit(function (e) {
        e.preventDefault();

        const formData = $(this).serialize();

        $.post(updateNoteUrl, formData, function (res) {
            if (res.success) {
                const mmss = new Date(res.note.timestamp * 1000).toISOString().substr(14, 5);
                const $li = $(`#note-${res.note.id}`);
                $li.find('.note-jump')
                    .text(res.note.content)
                    .data('timestamp', res.note.timestamp);
                $li.find('small').html(`<i class="fas fa-clock"></i> ${mmss}`);
                bootstrap.Modal.getInstance(document.getElementById('editNoteModal')).hide();
            } else {
                alert(res.message);
            }
        });
    });
});
