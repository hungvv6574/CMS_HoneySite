/// <reference path="../Src/knockout.validation.js" />

/************************************************
* This is an example localization page. All of these
* messages are the default messages for ko.validation
* 
* Currently ko.validation only does a single parameter replacement
* on your message (indicated by the {0}).
*
* The parameter that you provide in your validation extender
* is what is passed to your message to do the {0} replacement.
*
* eg: myProperty.extend({ minLength: 5 });
* ... will provide a message of "Please enter at least 5 characters"
* when validated
*
* This message replacement obviously only works with primitives
* such as numbers and strings. We do not stringify complex objects 
* or anything like that currently.
*/

ko.validation.localize({
    required: 'Trường bắt buộc nhập.',
    min: 'Vui lòng nhập một giá trị lớn hơn hoặc bằng {0}.',
    max: 'Vui lòng nhập một giá trị nhỏ hơn hoặc bằng {0}.',
    minLength: 'Vui lòng nhập ít nhất {0} ký tự.',
    maxLength: 'Vui lòng nhập không nhiều hơn {0} ký tự.',
    pattern: 'Vui lòng kiểm tra giá trị này.',
    step: 'Giá trị phải tăng bởi {0}',
    email: 'Đây không phải là một địa chỉ email đúng',
    date: 'Vui lòng nhập một ngày thích hợp',
    dateISO: 'Vui lòng nhập một ngày thích hợp',
    number: 'Vui lòng nhập một số',
    digit: 'Vui lòng nhập một chữ số',
    phoneUS: 'Vui lòng ghi rõ số điện thoại hợp lệ',
    equal: 'Giá trị phải bằng',
    notEqual: 'Hãy chọn giá trị khác.',
    unique: 'Hãy chắc chắn rằng giá trị là duy nhất.'
});