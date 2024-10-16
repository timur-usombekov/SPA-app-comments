import React, { useState } from 'react';
import axios from 'axios';
import './App.css';
import {
    FormGroup,
    InputGroup,
    Button,
    TextArea,
    Callout,
} from '@blueprintjs/core';

const resizeImage = (file, newWidth, newHeight) => {
    return new Promise((resolve, reject) => {
        const originalImage = new Image();
        const reader = new FileReader();

        reader.onload = (e) => {
            originalImage.src = e.target.result;
        };

        reader.readAsDataURL(file);

        originalImage.onload = () => {
            const originalWidth = originalImage.naturalWidth;
            const originalHeight = originalImage.naturalHeight;

          
            if (originalWidth <= newWidth && originalHeight <= newHeight) {
                resolve(file);
                return;
            }

            const canvas = document.createElement('canvas');
            const ctx = canvas.getContext('2d');
            const aspectRatio = originalWidth / originalHeight;

          
            if (!newHeight) {
                newHeight = Math.floor(newWidth / aspectRatio);
            }

            canvas.width = newWidth;
            canvas.height = newHeight;
            ctx.drawImage(originalImage, 0, 0, newWidth, newHeight);

            canvas.toBlob((blob) => {
                const resizedFile = new File([blob], file.name, {
                    type: file.type,
                    lastModified: Date.now(),
                });
                resolve(resizedFile);
            }, file.type, 0.9);
        };

        originalImage.onerror = (err) => {
            reject(err);
        };
    });
};

function CommentForm({ parentCommentId }) {
    const [formData, setFormData] = useState({
        username: '',
        email: '',
        text: '',
        url: null,
        file: null,
        captcha: '', 
        ParentCommentId: parentCommentId || '00000000-0000-0000-0000-000000000000',
    });

    const [errors, setErrors] = useState({});

    const handleChange = (e) => {
        const { name, value, type, files } = e.target;
        setFormData({
            ...formData,
            [name]: type === 'file' ? files[0] : value,
        });
    };

    const validateForm = async () => {
        const newErrors = {};

        if (!formData.username || !/^[a-zA-Z0-9]+$/.test(formData.username)) {
            newErrors.username = 'User Name can only contain letters and numbers.';
        }

        if (!formData.email || !/\S+@\S+\.\S+/.test(formData.email)) {
            newErrors.email = 'Please enter a valid email address.';
        }

        if (formData.url && !/^(https?:\/\/)?[\w.-]+(\.[a-z]{2,})+\/?/.test(formData.url)) {
            newErrors.homepage = 'Please enter a valid URL.';
        }

        const allowedTags = ['b', 'i', 'u', 'em', 'strong'];
        const regex = new RegExp(`</?(${allowedTags.join('|')})[^>]*>`, 'g');
        if (!formData.text || formData.text.replace(regex, '') === '') {
            newErrors.text = 'Please enter a valid comment text.';
        }

        if (formData.file) {
            if (formData.file.size > 100 * 1024 && formData.file.type === 'text/plain') {
                newErrors.file = 'TXT file size must not exceed 100 KB.';
            } else if (!/\.(jpg|txt|png|gif)$/i.test(formData.file.name)) {
                newErrors.file = 'File must be in JPG, GIF, TXT, or PNG format.';
            } else {
                const resizedFile = await resizeImage(formData.file, 320, 240);
                formData.file = resizedFile;
            }
        }

        if (formData.captcha !== 'smwm') {
            newErrors.captcha = 'Captcha is incorrect.';
        }

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const isValid = await validateForm();
        if (isValid) {
            const formDataToSubmit = new FormData();
            for (const key in formData) {
                if (formData[key] !== null) { 
                    formDataToSubmit.append(key, formData[key]);
                }
            }

            try {
                const response = await axios.post('http://localhost:8001/comment', formDataToSubmit);
                console.log('Data submitted successfully:', response.data);

                window.location.reload();

                setFormData({
                    username: '',
                    email: '',
                    text: '',
                    url: null,
                    file: null,
                    captcha: '',
                    ParentCommentId: parentCommentId || '00000000-0000-0000-0000-000000000000',
                });
                setErrors({});
            } catch (error) {
                console.error('Error submitting data:', error.response.data);
            }
        }
    };

    return (
        <form onSubmit={handleSubmit} className="commentForm-form">
            <FormGroup
                label="User Name"
                labelFor="username"
                helperText={errors.username}
                intent={errors.username ? 'danger' : 'none'}
            >
                <InputGroup
                    id="username"
                    name="username"
                    placeholder="Enter your name"
                    value={formData.username}
                    onChange={handleChange}
                    intent={errors.username ? 'danger' : 'none'}
                />
            </FormGroup>

            <FormGroup
                label="E-mail"
                labelFor="email"
                helperText={errors.email}
                intent={errors.email ? 'danger' : 'none'}
            >
                <InputGroup
                    id="email"
                    name="email"
                    placeholder="Enter your email"
                    value={formData.email}
                    onChange={handleChange}
                    intent={errors.email ? 'danger' : 'none'}
                />
            </FormGroup>

            <FormGroup
                label="Home page (optional)"
                labelFor="homepage"
                helperText={errors.homepage}
                intent={errors.homepage ? 'danger' : 'none'}
            >
                <InputGroup
                    id="homepage"
                    name="url"
                    placeholder="Enter your home page URL"
                    value={formData.url}
                    onChange={handleChange}
                    intent={errors.homepage ? 'danger' : 'none'}
                />
            </FormGroup>

            <FormGroup
                label="File Attachment (optional)"
                labelFor="file"
                helperText={errors.file}
                intent={errors.file ? 'danger' : 'none'}
            >
                <label className="custom-file-upload">
                    <input
                        type="file"
                        id="file"
                        name="file"
                        onChange={handleChange}
                        accept=".jpg, .gif, .png, .txt"
                    />
                    {formData.file ? formData.file.name : 'Choose file...'}
                </label>
            </FormGroup>

            <FormGroup
                label="Text"
                labelFor="text"
                helperText={errors.text ? errors.text + ' (allowed tags: <b>, <i>, <u>, <em>, <strong>)' : ''}
                intent={errors.text ? 'danger' : 'none'}
            >
                <TextArea
                    id="text"
                    name="text"
                    placeholder="Enter your comment"
                    value={formData.text}
                    onChange={handleChange}
                    intent={errors.text ? 'danger' : 'none'}
                    fill
                />
            </FormGroup>

            <img src="/Captcha.jpg" alt="captcha" />

            <FormGroup
                label="Captcha"
                labelFor="captcha"
                helperText={errors.captcha}
                intent={errors.captcha ? 'danger' : 'none'}
            >
                <InputGroup
                    id="captcha"
                    name="captcha"
                    placeholder="Enter the captcha"
                    value={formData.captcha}
                    onChange={handleChange}
                    intent={errors.captcha ? 'danger' : 'none'}
                />
            </FormGroup>

            {Object.keys(errors).length > 0 && (
                <Callout intent="danger" title="Validation Errors">
                    Please fix the above errors.
                </Callout>
            )}

            <Button type="submit" intent="primary" text="Submit" icon="send-message" />
        </form>
    );
}

export default CommentForm;
