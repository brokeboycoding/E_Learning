$(document).on('change', '.quiz-option', function () {
    const isCorrect = $(this).data('is-correct') === true || $(this).data('is-correct') === "true";
    const explanation = $(this).data('explanation');

    const $question = $(this).closest('.quiz-question');
    const $exp = $question.find('.explanation');

    if (isCorrect) {
        $exp.removeClass('text-danger').addClass('text-success');
        $exp.html(`✅ Chính xác!<br/><small>${explanation}</small>`);
    } else {
        $exp.removeClass('text-success').addClass('text-danger');
        $exp.html(`❌ Sai rồi!<br/><small>${explanation}</small>`);
    }

    $exp.show();
});