import React, { useEffect, useState } from 'react';
import axios from 'axios';

const CommentsList = () => {
    const [comments, setComments] = useState([]);

    useEffect(() => {
        const fetchComments = async () => {
            try {
                const response = await axios.get('https://localhost:7137/comment');
                setComments(response.data);
            } catch (error) {
                console.error('Load comments error:', error);
            }
        };

        fetchComments();
    }, []);

    return (
        <div>
            <h2>Comments</h2>
            {comments.length === 0 ? (
                <p>No comments available.</p>
            ) : (
                <ul>
                    {comments.map((comment) => (
                        <li key={comment.id}>
                            <p><strong>{comment.user.name}:</strong> {comment.text}</p>
                            <p><small>Email: {comment.user.email}</small></p>
                            <p><small>Created At: {new Date(comment.createdAt).toLocaleString()}</small></p>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
};

export default CommentsList;
