import React, { useEffect, useState } from 'react';
import axios from 'axios';
import CommentCard from './CommentCard';
import { Spinner, Callout } from '@blueprintjs/core';

const CommentList = () => {
    const [comments, setComments] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchComments = async () => {
            try {
                const response = await axios.get('https://localhost:7137/comment');

                const commentsWithFiles = response.data.map(comment => {
                    if (comment.file) {
                        const byteCharacters = atob(comment.file); 
                        const byteNumbers = new Uint8Array(byteCharacters.length);
                        for (let i = 0; i < byteCharacters.length; i++) {
                            byteNumbers[i] = byteCharacters.charCodeAt(i);
                        }
                        const blob = new Blob([byteNumbers], { type: 'application/octet-stream' });
                        const file = new File([blob], comment.fileName || 'attachment');
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

        fetchComments();
    }, []);

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
            {comments.length === 0 ? (
                <p>No comments found.</p>
            ) : (
                comments.map(comment => (
                    <CommentCard
                        key={comment.id}
                        username={comment.user.name}
                        email={comment.user.email}
                        text={comment.text}
                        file={comment.file}
                    />
                ))
            )}
        </div>
    );
};

export default CommentList;
