document.addEventListener('DOMContentLoaded', function () {
    const togglePassword = document.querySelector('#togglePassword');
    const password = document.querySelector('#password');

    togglePassword.addEventListener('click', function () {
        const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
        password.setAttribute('type', type);

        if (togglePassword.src.match("https://media.geeksforgeeks.org/wp-content/uploads/20210917150049/eyeslash.png")) {
            togglePassword.src = "https://media.geeksforgeeks.org/wp-content/uploads/20210917145551/eye.png";
        } else {
            togglePassword.src = "https://media.geeksforgeeks.org/wp-content/uploads/20210917150049/eyeslash.png";
        }
    });
});
