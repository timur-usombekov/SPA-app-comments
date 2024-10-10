import React, { useState } from 'react';
import { Card, Button, Icon, Spinner, Callout } from '@blueprintjs/core';
import axios from 'axios';
import './App.css';
import CommentForm from './CommentForm'; 

function CommentCard({ commentId, username, email, text, file, createdAt, fileExtension, url }) {
    const [replies, setReplies] = useState([]);
    const [loadingReplies, setLoadingReplies] = useState(false);
    const [error, setError] = useState(null);
    const [showReplies, setShowReplies] = useState(false);
    const [replying, setReplying] = useState(false);

    const fetchReplies = async () => {
        setLoadingReplies(true);
        setError(null);
        try {
            const response = await axios.get(`https://localhost:7137/comment/${commentId}`);

            if (response.data && Array.isArray(response.data)) {
                const repliesWithFiles = response.data.map(reply => {
                    if (reply.file) {
                        const byteCharacters = atob(reply.file);
                        const byteNumbers = new Uint8Array(byteCharacters.length);
                        for (let i = 0; i < byteCharacters.length; i++) {
                            byteNumbers[i] = byteCharacters.charCodeAt(i);
                        }
                        const blob = new Blob([byteNumbers], { type: 'application/octet-stream' });
                        const file = new File([blob], 'attachment');
                        return { ...reply, file };
                    }
                    return reply;
                });

                setReplies(repliesWithFiles);
            } else {
                setError('Invalid data format from API');
            }
        } catch (err) {
            setError('Failed to load replies: ' + err.message);
        } finally {
            setLoadingReplies(false);
        }
    };

    const handleShowReplies = () => {
        if (!showReplies) {
            fetchReplies();
        }
        setShowReplies(!showReplies);
    };

    const handleReply = () => {
        setReplying(!replying); 
    };

    return (
        <Card interactive={false} elevation={2} className="commentCard">
            <div className="commentCard-header">
                {url && (
                    <a href={url}>
                        <strong>{username}</strong>
                    </a>
                )}
                {!url && <strong>{username}</strong>}
                <span className="commentCard-email">({email})</span>
                <span className="commentCard-date">{new Date(createdAt).toLocaleString()}</span>
            </div>
            <p className="commentCard-text">{text}</p>
            {file && (
                <div className="commentCard-file">
                    {fileExtension !== ".txt" ? (
                        <img src={URL.createObjectURL(file)} alt="attachment" style={{ maxWidth: '100%', height: 'auto' }} />
                    ) : (
                        <a href={URL.createObjectURL(file)} target="_blank" rel="noopener noreferrer">
                            <Icon icon="document" /> {file.name}
                        </a>
                    )}
                    
                </div>
            )}
            <div className="commentCard-actions">
                <Button
                    intent="primary"
                    text="Reply"
                    onClick={handleReply} 
                />
                <Button
                    intent="success"
                    text={showReplies ? 'Hide replies' : 'Watch replies'}
                    onClick={handleShowReplies}
                />
            </div>

            {loadingReplies && (
                <div style={{ textAlign: 'center' }}>
                    <Spinner size={20} />
                    <p>Loading replies...</p>
                </div>
            )}

            {error && (
                <Callout intent="danger" title="Error">
                    {error}
                </Callout>
            )}

            {showReplies && replies.length > 0 && (
                <div className="commentCard-replies">
                    {replies.map(reply => (
                        <CommentCard
                            key={reply.id}
                            commentId={reply.id}
                            username={reply.user.name}
                            email={reply.user.email}
                            url={reply.url}
                            text={reply.text}
                            file={reply.file}
                            createdAt={reply.createdAt}
                        />
                    ))}
                </div>
            )}

            {showReplies && replies.length === 0 && !loadingReplies && (
                <p>No replies found.</p>
            )}

            {replying && (
                <CommentForm parentCommentId={commentId} />
            )}
        </Card>
    );
}

export default CommentCard;
