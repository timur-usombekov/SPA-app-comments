import React, { useEffect, useState } from 'react';
import axios from 'axios';
import CommentCard from './CommentCard';
import { Spinner, Callout, HTMLSelect } from '@blueprintjs/core';

const CommentList = () => {
    const [comments, setComments] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [sortBy, setSortBy] = useState('');

    const fetchComments = async (sortOption) => {
        setLoading(true);
        try {
            const url = sortOption ? `https://localhost:7137/comment?sortBy=${sortOption}` : 'https://localhost:7137/comment';
            const response = await axios.get(url);

            const commentsWithFiles = response.data.map(comment => {
                if (comment.file) {
                    const byteCharacters = atob(comment.file);
                    const byteNumbers = new Uint8Array(byteCharacters.length);
                    for (let i = 0; i < byteCharacters.length; i++) {
                        byteNumbers[i] = byteCharacters.charCodeAt(i);
                    }
                    const blob = new Blob([byteNumbers], { type: 'image/png' });
                    const file = new File([blob], 'attachment');
                    return { ...comment, file };
                }
                return comment;
            });

            setComments(commentsWithFiles);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchComments(sortBy);
    }, [sortBy]); 

    const handleSortChange = (event) => {
        setSortBy(event.target.value);
    };

    if (loading) {
        return (
            <div style={{ textAlign: 'center' }}>
                <Spinner />
                <p>Loading comments...</p>
            </div>
        );
    }

    if (error) {
        return (
            <Callout intent="danger" title="Error">
                Error loading comments: {error}
            </Callout>
        );
    }

    return (
        <div>
            <div style={{ marginBottom: '15px' }}>
                <HTMLSelect onChange={handleSortChange} value={sortBy}>
                    <option value="">Sort by Date(descending)</option>
                    <option value="date">Sort by Date(ascending)</option>
                    <option value="username">Sort by Username</option>
                    <option value="email">Sort by Email</option>
                </HTMLSelect>
            </div>

            {comments.length === 0 ? (
                <p>No comments found.</p>
            ) : (
                comments.map(comment => (
                    <CommentCard
                        key={comment.id}
                        commentId={comment.id}
                        username={comment.user.name}
                        email={comment.user.email}
                        text={comment.text}
                        createdAt={comment.createdAt}
                        file={comment.file}
                        fileExtension={comment.fileExtension}
                        url={comment.url}
                    />
                ))
            )}
        </div>
    );
};

export default CommentList;
