jQuery.noConflict();
(function (bot) {
    var phoneNumber;
    var message;
    var friendsForSend;
    var arrFriends = [];
    var arrItem = [];
    var tryCountCommon = 5;
    var findItemTryCount = tryCountCommon;
    var getFriendsTryCount = 3;
    var getFriendsOnSearchTryCount = tryCountCommon;
    var findMenuDanhSachKetBanTryCount = tryCountCommon;
    var findTbxSearchTryCount = tryCountCommon;
    var findTbxChatTryCount = tryCountCommon;
    var delaySecond;
    var zaloFriendInviteStatus;
    var zaloFriendMessageStatus;

    bot.init = function (_phoneNumber, _zaloFriendInviteStatus, _zaloFriendMessageStatus) {
        phoneNumber = _phoneNumber;
        zaloFriendInviteStatus = _zaloFriendInviteStatus;

        zaloFriendMessageStatus = _zaloFriendMessageStatus;

        (async function () {
            await CefSharp.BindObjectAsync("botgui");
        })();
        //botgui.log("bot.init => " + window.location.href);
    };

    bot.getZaloFriendInviteStatus = function (name) {
        var arr = zaloFriendInviteStatus.filter(x => x.name === name);
        if (arr.length > 0) return arr[0].value;

        return "";
    };

    bot.getZaloFriendMessageStatus = function (name) {
        var arr = zaloFriendMessageStatus.filter(x => x.name === name);
        if (arr.length > 0) return arr[0].value;

        return "";
    };

    bot.checkLogin = function (isAuto) {

        if (typeof botgui === "undefined") {
            setTimeout(function () {
                (async function () {
                    await CefSharp.BindObjectAsync("botgui");
                    bot.checkLogin(isAuto);
                })();
            }, 1000);
            return;
        }

        if (bot.isInLoginPage()) {
            if (isAuto) {
                botgui.loginFail();
            }
        }
        else {
            //khi mới vào web, thì link đầu tiên sẽ là chat.zalo.me, do đó để đảm bảo đã login thì setTimeout là 3 giấy sau
            //nếu web không chuyển sang id.zalo.me, thì chính xác là đã login, còn web chuyển sang id.zalo.me thì hàm setTimeout này sẽ bị clear
            setTimeout(function () {
                botgui.loginSuccess();
            }, 2000);
        }
    };

    bot.isInLoginPage = function () {
        return window.location.href.indexOf("https://chat.zalo.me/login") === 0 || window.location.href.indexOf("https://id.zalo.me/") === 0;
    };

    bot.isLoggedIn = function () {
        if (typeof botgui === "undefined") {
            setTimeout(function () {
                (async function () {
                    await CefSharp.BindObjectAsync("botgui");
                    bot.isLoggedIn();
                })();
            }, 1000);
            return;
        }

        if (window.location.href.indexOf("https://chat.zalo.me") >= 0) {
            botgui.loginSuccess();
        }
    };

    /*- start send message -*/

    bot.sendMessage = function (msg, _friendsForSend, _delaySecond) {

        if (typeof botgui === "undefined") {
            setTimeout(function () {
                (async function () {
                    await CefSharp.BindObjectAsync("botgui");
                    bot.sendMessage(msg, _friendsForSend, _delaySecond);
                })();
            }, 1000);
            return;
        }

        if (msg !== "") {
            message = decodeURIComponent(msg);
        }

        if (_friendsForSend !== null) {
            friendsForSend = _friendsForSend;
        }

        if (_delaySecond !== -1) {
            delaySecond = _delaySecond;
        }

        if (bot.isInLoginPage()) {
            botgui.sendMessageComplete("chưa login");
        } else {
            botgui.log(phoneNumber + " starting send message, please wait ...");
            jQuery('[data-translate-title="STR_TAB_CONTACT"]').click();
            setTimeout(function () {
                var callback = function () {
                    if (arrFriends.length === 0) {
                        if (getFriendsTryCount > 0) {
                            getFriendsTryCount -= 1;
                            setTimeout(function () {
                                bot.sendMessage("", null, -1);
                            }, 1000);
                        } else {
                            botgui.sendMessageComplete("danh sách bạn bè trống");
                        }
                    } else {
                        bot.searchFriendForSendMessage();
                    }
                };
                bot.doGetFriends(callback);
            }, 1000);
        }
    };

    bot.searchFriendForSendMessage = function () {
        if (arrFriends.length === 0) {
            botgui.sendMessageComplete("");
            return;
        }

        var friend = arrFriends.shift();

        var isMustSend = friendsForSend.filter(name => {
            var replaceWhat = "%27";
            var newName = name.replace(replaceWhat, "'", "g");

            return friend.name === newName;
        }).length > 0;

        if (!isMustSend) {
            setTimeout(function () {
                bot.searchFriendForSendMessage();
            }, 10);
            return;
        }

        bot.setValueTextBox(jQuery('[data-translate-title="STR_CONTACT_TAB_SEARCH_FRIEND"]'), friend.name);
        setTimeout(function () {
            jQuery(".data-list__more").click();

            setTimeout(function () {
                getFriendsOnSearchTryCount = tryCountCommon;
                bot.getFriendsOnSearchForSendMessage(friend);
            }, 1000);
        }, 2000);
    };

    bot.getFriendsOnSearchForSendMessage = function (friend) {
        arrItem = [];
        var divOutter = jQuery("#searchResultList");
        divOutter.find(".item").each(function () {
            var item = jQuery(this);
            if (item.find(".item-message").length > 0) return;
            if (friend.key === bot.getReactKey(item.get(0))) {
                arrItem.push({
                    div: item,
                    friend: friend
                });
            }
        });

        if (arrItem.length > 0) {
            bot.sendMessageForOne();
        } else if (getFriendsOnSearchTryCount > 0) {
            getFriendsOnSearchTryCount -= 1;
            setTimeout(function () {
                bot.getFriendsOnSearchForSendMessage(friend);
            }, 10);
        } else {
            setTimeout(function () {
                botgui.log(phoneNumber + " " + bot.getZaloFriendMessageStatus("FriendNotFound") + " " + friend.name);
                botgui.updateMessagePerFriendStatus(friend.name, bot.getZaloFriendMessageStatus("FriendNotFound"));
                bot.searchFriendForSendMessage();
            }, delaySecond * 1000);
        }
    };

    bot.sendMessageForOne = function () {
        if (arrItem.length === 0) {
            setTimeout(function () {
                bot.searchFriendForSendMessage();
            }, delaySecond * 1000);
            return;
        }

        var item = arrItem.shift();
        var div = item.div;
        var friend = item.friend;

        var clickTextBox = function () {
            var el = jQuery("#richInput");
            if (el.length === 0) {
                if (findTbxChatTryCount > 0) {
                    findTbxChatTryCount -= 1;
                    setTimeout(function () {
                        clickTextBox();
                    }, 1000);
                } else {
                    botgui.updateMessagePerFriendStatus(friend.name, bot.getZaloFriendMessageStatus("ZaloGUIError") + "Không tìm thấy textbox chat");
                    setTimeout(function () {
                        bot.sendMessageForOne();
                    }, 10);
                }
            } else {
                el.attr("contenteditable", "true");

                bot.setValueTextBox_Div_Contenteditable(el.get(0), bot.buildMessageHtml(friend.name));

                setTimeout(function () {
                    var rect = jQuery("#sendBtn").get(0).getBoundingClientRect();
                    var x = parseInt(rect.x) + 1;
                    var y = parseInt(rect.y) + 1;
                    botgui.clickOnBrowser(x, y);

                    //kiểm tra xem zalo chặn gửi tin hay không
                    setTimeout(function () {
                        var lastChatItem = jQuery("#messageViewScroll").find(".chat-item:last");
                        if (lastChatItem.hasClass("me")) {
                            //zalo không chặn
                            botgui.updateMessagePerFriendStatus(friend.name, bot.getZaloFriendMessageStatus("Sent"));

                            botgui.log(phoneNumber + " đã gửi tin cho " + friend.name);
                        } else {
                            //zalo chặn gửi tin
                            botgui.updateMessagePerFriendStatus(friend.name, bot.getZaloFriendMessageStatus("ZaloDenied"));

                            botgui.log(phoneNumber + " " + bot.getZaloFriendMessageStatus("ZaloDenied") + " " + friend.name);
                        }

                        setTimeout(function () {
                            bot.sendMessageForOne();
                        }, 10);
                    }, 2000);
                }, 1000);
            }
        };

        (async function () {
            await bot.simulateMouseClick(div.get(0));
            setTimeout(function () {
                clickTextBox();
            }, 1000);

        })();
    };

    /*- end send message -*/

    /*- start send message to phone numbers -*/

    bot.sendMessageToPhoneNumber = function (msg, _friendsForSend, _delaySecond) {

        if (typeof botgui === "undefined") {
            setTimeout(function () {
                (async function () {
                    await CefSharp.BindObjectAsync("botgui");
                    bot.sendMessageToPhoneNumber(msg, _friendsForSend, _delaySecond);
                })();
            }, 1000);
            return;
        }

        if (msg !== "") {
            message = decodeURIComponent(msg);
        }

        if (_friendsForSend !== null) {
            friendsForSend = _friendsForSend;
        }

        if (_delaySecond !== -1) {
            delaySecond = _delaySecond;
        }

        if (bot.isInLoginPage()) {
            botgui.sendMessageToPhoneNumberComplete("chưa login");
        } else {
            botgui.log(phoneNumber + " starting send message to phone number, please wait ...");
            bot.searchFriendSendMessageToPhoneNumber();
        }
    };

    bot.searchFriendSendMessageToPhoneNumber = function () {
        if (friendsForSend.length === 0) {
            botgui.sendMessageToPhoneNumberComplete("");
            return;
        }

        if (jQuery("#contact-search-input").length === 0) {
            if (findTbxSearchTryCount > 0) {
                findTbxSearchTryCount -= 1;
                setTimeout(function () {
                    bot.searchFriendSendMessageToPhoneNumber();
                }, 1000);
            } else {
                botgui.sendMessageToPhoneNumberComplete("Không tìm thấy textbox search");
            }
            return;
        }

        var friendPhoneNumber = friendsForSend.shift();
        bot.setValueTextBox(jQuery("#contact-search-input").get(0), friendPhoneNumber);

        setTimeout(function () {
            //click button "xem thêm" nếu có
            jQuery(".data-list__more").click();

            setTimeout(function () {
                getFriendsOnSearchTryCount = tryCountCommon;
                bot.getFriendsOnSearchForSendMessageToPhoneNumber(friendPhoneNumber);
            }, 1000);
        }, 2000);
    };

    bot.getFriendsOnSearchForSendMessageToPhoneNumber = function (friendPhoneNumber) {
        arrItem = [];
        var divOutter = jQuery("#searchResultList");
        divOutter.find(".item").each(function () {
            var item = jQuery(this);
            if (item.find(".item-message").length > 0) return;

            var name = item.find(".friend_online_status").parent().find("div.truncate").text();
            arrItem.push({
                div: item,
                friendPhoneNumber: friendPhoneNumber,
                friendName: name
            });
        });

        if (arrItem.length > 0) {
            bot.sendMessageToPhoneNumberForOne();
        } else if (getFriendsOnSearchTryCount > 0) {
            getFriendsOnSearchTryCount -= 1;
            setTimeout(function () {
                bot.getFriendsOnSearchForSendMessageToPhoneNumber(friendPhoneNumber);
            }, 1000);
        } else {
            //tìm không thấy friend, perform update status
            botgui.log(phoneNumber + " " + bot.getZaloFriendMessageStatus("FriendNotFound") + " " + friendPhoneNumber);
            botgui.updateSendMessageToPhoneNumberPerFriendStatus(friendPhoneNumber, bot.getZaloFriendMessageStatus("FriendNotFound"));
            setTimeout(function () {
                bot.searchFriendSendMessageToPhoneNumber();
            }, delaySecond * 1000);
        }
    };

    bot.sendMessageToPhoneNumberForOne = function () {
        if (arrItem.length === 0) {
            setTimeout(function () {
                bot.searchFriendSendMessageToPhoneNumber();
            }, delaySecond * 1000);
            return;
        }
        var item = arrItem.shift();
        var div = item.div;
        var friendPhoneNumber = item.friendPhoneNumber;
        var friendName = item.friendName;

        findTbxChatTryCount = tryCountCommon;

        var clickTextBox = function () {
            var el = jQuery("#richInput");
            if (el.length === 0) {
                if (findTbxChatTryCount > 0) {
                    findTbxChatTryCount -= 1;
                    setTimeout(function () {
                        clickTextBox();
                    }, 1000);
                } else {
                    botgui.updateSendMessageToPhoneNumberPerFriendStatus(friendPhoneNumber, bot.getZaloFriendMessageStatus("ZaloGUIError") + "Không tìm thấy textbox chat");
                    setTimeout(function () {
                        bot.sendMessageToPhoneNumberForOne();
                    }, 10);
                }
            } else {
                el.attr("contenteditable", "true");

                bot.setValueTextBox_Div_Contenteditable(el.get(0), bot.buildMessageHtml(friendName));

                setTimeout(function () {
                    var rect = jQuery("#sendBtn").get(0).getBoundingClientRect();
                    var x = parseInt(rect.x) + 1;
                    var y = parseInt(rect.y) + 1;
                    botgui.clickOnBrowser(x, y);

                    //kiểm tra xem zalo chặn gửi tin hay không
                    setTimeout(function () {
                        var lastChatItem = jQuery("#messageViewScroll").find(".chat-item:last");
                        if (lastChatItem.hasClass("me")) {
                            //zalo không chặn
                            botgui.updateSendMessageToPhoneNumberPerFriendStatus(friendPhoneNumber, bot.getZaloFriendMessageStatus("Sent"));
                            botgui.log(phoneNumber + " đã gửi tin cho " + friendPhoneNumber);
                        } else {
                            //zalo chặn gửi tin
                            botgui.updateSendMessageToPhoneNumberPerFriendStatus(friendPhoneNumber, bot.getZaloFriendMessageStatus("ZaloDenied"));
                            botgui.log(phoneNumber + " " + bot.getZaloFriendMessageStatus("ZaloDenied") + " " + friendPhoneNumber);
                        }

                        setTimeout(function () {
                            bot.sendMessageToPhoneNumberForOne();
                        }, 10);
                    }, 2000);
                }, 1000);
            }
        };

        (async function () {
            await bot.simulateMouseClick(div.get(0));
            setTimeout(function () {
                clickTextBox();
            }, 1000);

        })();
    };

    /*- end send message to phone numbers -*/

    /*- start send invite -*/

    bot.sendInvite = function (msg, _friendsForSend, _delaySecond) {

        if (typeof botgui === "undefined") {
            setTimeout(function () {
                (async function () {
                    await CefSharp.BindObjectAsync("botgui");
                    bot.sendInvite(msg, _friendsForSend, _delaySecond);
                })();
            }, 1000);
            return;
        }

        if (msg !== "") {
            message = decodeURIComponent(msg);
        }

        if (_friendsForSend !== null) {
            friendsForSend = _friendsForSend;
        }

        if (_delaySecond !== -1) {
            delaySecond = _delaySecond;
        }

        if (bot.isInLoginPage()) {
            botgui.sendInviteComplete("chưa login");
        } else {
            botgui.log(phoneNumber + " starting send invite, please wait ...");
            bot.searchFriendSendInvite();
        }
    };

    bot.searchFriendSendInvite = function () {
        if (friendsForSend.length === 0) {
            botgui.sendInviteComplete("");
            return;
        }

        if (jQuery("#contact-search-input").length === 0) {
            if (findTbxSearchTryCount > 0) {
                findTbxSearchTryCount -= 1;
                setTimeout(function () {
                    bot.searchFriendSendInvite();
                }, 1000);
            } else {
                botgui.sendInviteComplete("Không tìm thấy textbox search");
            }
            return;
        }

        var friendPhoneNumber = friendsForSend.shift();
        bot.setValueTextBox(jQuery("#contact-search-input").get(0), friendPhoneNumber);

        setTimeout(function () {
            //click button "xem thêm" nếu có
            jQuery(".data-list__more").click();

            setTimeout(function () {
                getFriendsOnSearchTryCount = tryCountCommon;
                bot.getFriendsOnSearchForSendInvite(friendPhoneNumber);
            }, 1000);
        }, 2000);
    };

    bot.getFriendsOnSearchForSendInvite = function (friendPhoneNumber) {
        arrItem = [];
        var divOutter = jQuery("#searchResultList");
        divOutter.find(".item").each(function () {
            var item = jQuery(this);
            if (item.find(".item-message").length > 0) return;

            var name = item.find(".friend_online_status").parent().find("div.truncate").text();
            arrItem.push({
                div: item,
                friendPhoneNumber: friendPhoneNumber,
                friendName: name
            });
        });

        if (arrItem.length > 0) {
            bot.sendInviteForOne();
        } else if (getFriendsOnSearchTryCount > 0) {
            getFriendsOnSearchTryCount -= 1;
            setTimeout(function () {
                bot.getFriendsOnSearchForSendInvite(friendPhoneNumber);
            }, 1000);
        } else {
            //tìm không thấy friend, perform update status
            botgui.log(phoneNumber + " " + bot.getZaloFriendInviteStatus("FriendNotFound") + " " + friendPhoneNumber);
            botgui.updateInvitePerFriendStatus(friendPhoneNumber, bot.getZaloFriendInviteStatus("FriendNotFound"));
            setTimeout(function () {
                bot.searchFriendSendInvite();
            }, delaySecond * 1000);
        }
    };

    bot.sendInviteForOne = function () {
        if (arrItem.length === 0) {
            setTimeout(function () {
                bot.searchFriendSendInvite();
            }, delaySecond * 1000);
            return;
        }
        var item = arrItem.shift();
        var div = item.div;
        var friendPhoneNumber = item.friendPhoneNumber;
        var friendName = item.friendName;

        findTbxChatTryCount = tryCountCommon;

        var clickTextBox = function () {
            var el = jQuery("#richInput");
            if (el.length === 0) {
                if (findTbxChatTryCount > 0) {
                    findTbxChatTryCount -= 1;
                    setTimeout(function () {
                        clickTextBox();
                    }, 1000);
                } else {
                    botgui.updateInvitePerFriendStatus(friendPhoneNumber, bot.getZaloFriendInviteStatus("ZaloGUIError") + "Không tìm thấy textbox chat");
                    setTimeout(function () {
                        bot.sendInviteForOne();
                    }, 10);
                }
            } else {
                var btnAdd = jQuery('#messageView').find('[data-translate-inner="STR_PROFILE_ADD_FRIEND"]');
                if (btnAdd.length > 0) {
                    //chưa add friend
                    botgui.log(phoneNumber + " đang gửi lời mời " + friendPhoneNumber);
                    (async function () {
                        await bot.simulateMouseClick(btnAdd.get(0));
                        setTimeout(function () {
                            var txtArea = jQuery(".friend-profile__addfriend__msg");
                            bot.setValueTextBox(txtArea.get(0), bot.buildMessgeContent(message, friendName));

                            (async function () {
                                await bot.simulateMouseClick(document.getElementById("_sendRequest"));

                                setTimeout(function () {
                                    btnAdd = jQuery('#messageView').find('[data-translate-inner="STR_PROFILE_ADD_FRIEND"]');
                                    if (btnAdd.length > 0) {
                                        //zalo chặn kết bạn
                                        botgui.updateInvitePerFriendStatus(friendPhoneNumber, bot.getZaloFriendInviteStatus("ZaloDenied"));
                                    } else {
                                        //đã gửi lời mời kết bạn
                                        botgui.updateInvitePerFriendStatus(friendPhoneNumber, bot.getZaloFriendInviteStatus("Sent"));
                                    }
                                    bot.sendInviteForOne();
                                }, 2000);
                            })();
                        }, 2000);
                    })();
                } else {
                    if (jQuery("#messageView").find('[data-translate-inner="STR_PROFILE_FRIEND_REQ_SENT"]').length > 0) {
                        botgui.updateInvitePerFriendStatus(friendPhoneNumber, bot.getZaloFriendInviteStatus("ZaloWebShowInviteIsPending"));
                        botgui.log(phoneNumber + " đã gửi lời mời trước đó " + friendPhoneNumber);
                    } else {
                        botgui.updateInvitePerFriendStatus(friendPhoneNumber, bot.getZaloFriendInviteStatus("NotSendBecauseAdded"));
                        botgui.log(phoneNumber + " friend đã add " + friendPhoneNumber);
                    }
                    setTimeout(function () {
                        bot.sendInviteForOne();
                    }, 10);
                }
            }
        };

        (async function () {
            await bot.simulateMouseClick(div.get(0));
            setTimeout(function () {
                clickTextBox();
            }, 1000);

        })();
    };

    /*- end send invite -*/

    /*- start get friends for accept friend -*/

    bot.getFriendsForAcceptFriends = function () {
        botgui.log("start get friends for accept friends");
        if (typeof botgui === "undefined") {
            setTimeout(function () {
                (async function () {
                    await CefSharp.BindObjectAsync("botgui");
                    bot.getFriendsForAcceptFriends();
                })();
            }, 1000);
            return;
        }

        if (bot.isInLoginPage()) {
            botgui.updateFriendListForAcceptFriend("", "chưa login");
        } else {
            botgui.log(phoneNumber + " getting friends, please wait ...");

            var item = jQuery("#conversationListId").find('[data-translate-inner="STR_CONTACT_SUGGEST_FRIEND_2"]');

            if (item.length > 0) {
                bot.simulateMouseClick(item.get(0));

                setTimeout(function () {

                    jQuery(".friend-center-main").find(".fr-viewmore__text-more").each(function () {
                        var span = jQuery(this);
                        if (span.parent().parent().find('[data-translate-inner="STR_FRIEND_REQUEST_DEFAULT"]').length > 0) {
                            bot.simulateMouseClick(span.get(0));
                        }
                    });

                    var callback = function () {
                        if (arrFriends.length === 0) {
                            if (findItemTryCount > 0) {
                                findItemTryCount -= 1;
                                bot.getFriendsForAcceptFriends();
                            } else {
                                botgui.updateFriendListForAcceptFriend(JSON.stringify(arrFriends), "");
                            }
                        } else {
                            botgui.updateFriendListForAcceptFriend(JSON.stringify(arrFriends), "");
                        }
                    };
                    bot.doGetFriendsForAcceptFriends(callback);
                }, 1000);
            } else {
                if (findMenuDanhSachKetBanTryCount > 0) {
                    findMenuDanhSachKetBanTryCount -= 1;
                    setTimeout(function () {
                        bot.getFriendsForAcceptFriends();
                    }, 1000);
                } else {
                    botgui.updateFriendListForAcceptFriend("", "");
                }
            }
        }
    };

    bot.doGetFriendsForAcceptFriends = function (callback) {
        var divOutter = jQuery("#react-v-list");

        var timer1 = null;
        var timer2 = null;

        var doGet = function () {
            divOutter.find(".friend-center-item").each(function () {
                var item = jQuery(this);
                var key = bot.getReactKey(item.parent().get(0));
                var name = item.find("div.truncate").text();
                var image = item.find(".avatar-img").css('background-image').replace(/^url\(['"](.+)['"]\)/, '$1');
                var arr = arrFriends.filter(i => i.key === key);
                if (arr.length === 0) {
                    arrFriends.push({
                        name: name,
                        image: image,
                        key: key
                    });
                }
            });
        };

        divOutter.scroll(function () {
            doGet();
        });

        doGet();
        timer1 = setInterval(function () {
            divOutter.animate({ scrollTop: divOutter.prop("scrollHeight") }, 2000);
        }, 4000);

        var friendsCount = 0;
        var countDown = 1;
        timer2 = setInterval(function () {
            if (friendsCount !== arrFriends.length) {
                friendsCount = arrFriends.length;
            } else {
                if (countDown <= 0) {
                    clearInterval(timer1);
                    clearInterval(timer2);

                    callback();
                } else {
                    countDown -= 1;
                }
            }
        }, 4000);
    };

    /*- end get friends for accept friend -*/

    /*- start accept friend -*/

    bot.acceptFriends = function (_friendsForSend) {

        if (typeof botgui === "undefined") {
            setTimeout(function () {
                (async function () {
                    await CefSharp.BindObjectAsync("botgui");
                    bot.acceptFriends(_friendsForSend);
                })();
            }, 1000);
            return;
        }

        if (_friendsForSend !== null) {
            friendsForSend = _friendsForSend;
        }

        var item = jQuery('.msg-item span[data-translate-inner="STR_CONTACT_SUGGEST_FRIEND_2"]');

        if (item.length > 0) {
            bot.simulateMouseClick(item.get(0));

            setTimeout(function () {
                bot.acceptFriendOne();
            }, 1000);
        } else {
            if (findItemTryCount > 0) {
                findItemTryCount -= 1;
                setTimeout(function () {
                    bot.acceptFriends(_friendsForSend);
                }, 1000);
            } else {
                botgui.acceptFriendsCompleted();
            }
        }
    };

    bot.acceptFriendOne = function () {
        arrFriends = [];
        if (friendsForSend.length === 0) {
            botgui.acceptFriendsCompleted();
            return;
        }

        botgui.log("start accept one");
        var friendName = friendsForSend.shift();

        var divOutter = jQuery("#react-v-list");

        //scroll and find div item have key match friend.key
        var timer3 = null;
        var timer2 = null;

        var doGet = function () {
            divOutter.find(".friend-center-item").each(function () {
                var item = jQuery(this);
                var key = bot.getReactKey(item.parent().get(0));
                var name = item.find("div.truncate").text();
                var image = item.find(".avatar-img").css('background-image').replace(/^url\(['"](.+)['"]\)/, '$1');
                var arr = arrFriends.filter(i => i.key === key);
                if (arr.length === 0) {
                    arrFriends.push({
                        name: name,
                        image: image,
                        key: key
                    });
                }
            });
        };

        //click button "Xem thêm"
        jQuery(".friend-center-main").find(".fr-viewmore__text-more").each(function () {
            var span = jQuery(this);
            if (span.parent().parent().find('[data-translate-inner="STR_FRIEND_REQUEST_DEFAULT"]').length > 0) {
                bot.simulateMouseClick(span.get(0));
            }
        });

        var doAccept = function () {
            divOutter.find(".friend-center-item").each(function () {
                var item = jQuery(this);
                var name = item.find("div.truncate").text();
                if (name === friendName) {
                    if (timer2 !== null) {
                        clearInterval(timer2);
                        timer2 = null;
                    }

                    if (timer3 !== null) {
                        clearInterval(timer3);
                        timer3 = null;
                    }

                    bot.clickAcceptFriend(item, name);
                }
            });
        };

        divOutter.scroll(function () {
            doGet();
            doAccept();
        });

        timer3 = setInterval(function () {
            divOutter.animate({ scrollTop: divOutter.prop("scrollHeight") }, 2000);
        }, 4000);

        doAccept();

        if (timer3 !== null) {
            divOutter.animate({ scrollTop: divOutter.prop("scrollHeight") }, 2000);
        }

        var friendsCount = 0;
        var countDown = 1;
        timer2 = setInterval(function () {
            if (friendsCount !== arrFriends.length) {
                friendsCount = arrFriends.length;
            } else {
                if (countDown <= 0) {
                    if (timer3 !== null) {
                        clearInterval(timer3);
                        timer3 = null;
                    }

                    if (timer2 !== null) {
                        clearInterval(timer2);
                        timer2 = null;
                    }

                    setTimeout(function () {
                        bot.acceptFriends(null);
                    }, 1000);
                } else {
                    countDown -= 1;
                }
            }
        }, 4000);
    };

    bot.clickAcceptFriend = function (div, name) {
        botgui.log("click accept friends " + name);

        var btn = div.find('[data-translate-inner="STR_CONFIRM_DIALOG_ACCEPT"]');

        bot.simulateMouseClick(btn.get(0));

        setTimeout(function () {
            bot.acceptFriends(null);
        }, 1000);
    };

    /*- end accept friend -*/

    /*- start get contacts -*/

    bot.getFriends = function () {

        if (typeof botgui === "undefined") {
            setTimeout(function () {
                (async function () {
                    await CefSharp.BindObjectAsync("botgui");
                    bot.getFriends();
                })();
            }, 1000);
            return;
        }

        if (bot.isInLoginPage()) {
            botgui.updateFriendList("", "chưa login");
        } else {
            botgui.log(phoneNumber + " getting friends, please wait ....");
            jQuery('[data-translate-title="STR_TAB_CONTACT"]').click();
            setTimeout(function () {
                var callback = function () {
                    if (arrFriends.length === 0) {
                        if (getFriendsTryCount > 0) {
                            getFriendsTryCount -= 1;
                            setTimeout(function () {
                                bot.getFriends();
                            }, 1000);
                        } else {
                            botgui.updateFriendListEmpty("");
                        }
                    } else {
                        botgui.updateFriendList(JSON.stringify(arrFriends), "");
                    }
                };
                bot.doGetFriends(callback);
            }, 1000);
        }
    };

    /*
     * Danh sách bạn bè được zalo ẩn bớt, khi user scroll danh sách, thì zalo sẽ hiển thị thêm, nhưng ko hiển thị hết, và xóa bỏ những cái đã bị scoll che mất
     * Do đó thuật toán ở đây là sẽ bắt sự kiện scroll của danh sách bạn bè, khi scroll thì thêm bạn bè vào, nếu trùng thì ko thêm, sau đó tiếp tục scroll cho
     * đến hết danh sách
     */
    bot.doGetFriends = function (callback) {
        var scrollableContainer;
        var listWrapper;
        var scrollStep = 500;
        var scrollIntervalTime = 0.005; // seconds
        var stopAction = false;

        // Lấy tab liên hệ và nhấp vào nếu tồn tại
        var tabContact = jQuery(".clickable.leftbar-tab").filter(function () {
            return jQuery(this).data("translate-title") === "STR_TAB_CONTACT";
        });

        if (tabContact.length > 0) {
            tabContact.click();
            console.log("tabContact.click() OK");
        } else {
            console.log("tabContact.click() Failed");
        }

        // Hàm lấy danh sách bạn bè
        var getWrapper = function () {
            scrollableContainer = jQuery(".ReactVirtualized__Grid.ReactVirtualized__List.contact-tab-v2__list-custom").parent();
            scrollableContainer.scrollTop(100);
            listWrapper = scrollableContainer.find(".ReactVirtualized__Grid__innerScrollContainer");
            console.log(listWrapper);
        };

        // Hàm trích xuất bạn bè từ danh sách
        var extractFriendFromList = function () {
            if (!listWrapper) {
                console.log("listWrapper == null");
                return;
            }

            var siblingDivs = listWrapper.children().not('.card-list-title');
            siblingDivs.children('.contact-item-v2-wrapper').each(function () {
                var item = jQuery(this);
                var name = item.find(".friend-info .name-wrapper .name").text();
                var image = item.find(".friend-info .zavatar-container img").attr('src');

                if (name && image) {
                    var key = image.substring(image.lastIndexOf('/') + 1).replace('.jpg', '');
                    if (!arrFriends.some(function (i) { return i.key === key; })) {
                        arrFriends.push({ name: name, image: image, key: key });
                        console.log("push " + name);
                    }
                }
            });
        };

        // Khởi động việc lấy wrapper và thiết lập scroll event
        setTimeout(function () {
            getWrapper();

            var scrollPosition = 0;
            var intervalJob = setInterval(function () {
                console.log("%c Start intervalJob", 'background: #222; color: #bada55');

                var lastObj = jQuery(".contact-item-v2-wrapper.last");

                extractFriendFromList();
                if (lastObj.length <= 0) {
                    scrollPosition += scrollStep;
                    scrollableContainer.scrollTop(scrollPosition);

                } else {
                    clearInterval(intervalJob);
                    callback();
                    console.log("%c Stop intervalJob ", 'background: #222; color: #bada55');
                    console.log("%c Number " + arrFriends.length, 'background: #222; color: #bada55');
                }
            }, scrollIntervalTime * 1000);
        }, 100);
    };
    /*- end get contacts -*/

    bot.buildMessageHtml = function (friendName) {
        var html = "";
        var arr = message.split("[<br/>]");
        for (var i = 0; i < arr.length; ++i) {
            html += `<div id="input_line_${i}">` + arr[i] + "</div>";
        }

        html = bot.buildMessgeContent(html, friendName);

        return html;
    };

    bot.buildMessgeContent = function (text, friendName) {
        text = text.replace('{tên}', friendName, "g");
        text = text.replace('{Tên}', friendName, "g");
        text = text.replace('{Ten}', friendName, "g");
        text = text.replace('{ten}', friendName, "g");
        return text;
    };

    bot.removeAllWhiteSpaces = function (val) {
        return val.replace(/\s/g, "");
    };

    bot.setValueTextBox = function (input, value) {
        let lastValue = input.value;
        input.value = value;
        let event = new Event('input', { bubbles: true });
        // hack React15
        event.simulated = true;
        // hack React16 内部定义了descriptor拦截value，此处重置状态
        let tracker = input._valueTracker;
        if (tracker) {
            tracker.setValue(lastValue);
        }
        input.dispatchEvent(event);
    };

    bot.setValueTextBox_Div_Contenteditable = function (input, value) {
        let lastValue = input.innerHTML;
        input.innerHTML = value;
        let event = new Event('input', { bubbles: true });
        // hack React15
        event.simulated = true;
        // hack React16 内部定义了descriptor拦截value，此处重置状态
        let tracker = input._valueTracker;
        if (tracker) {
            tracker.setValue(lastValue);
        }
        input.dispatchEvent(event);
    };

    bot.getReactKey = function (dom) {
        for (var key in dom) {
            if (key.startsWith("__reactInternalInstance$")) {
                return dom[key].key;
            }
        }
        return "";
    };

    bot.simulateMouseClick = async function (el) {
        if (typeof el === "undefined") return;
        let opts = { view: window, bubbles: true, cancelable: true, buttons: 1 };
        el.dispatchEvent(new MouseEvent("mousedown", opts));
        await new Promise(r => setTimeout(r, 50));
        el.dispatchEvent(new MouseEvent("mouseup", opts));
        el.dispatchEvent(new MouseEvent("click", opts));
    };

}(window.bot = window.bot || {}));

