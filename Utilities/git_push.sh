git_user_id = $1;
git_repo_id = $2;
release_note = $3;

if ["$git_user_id" = ""]; then
	git_user_id = ""
	echo "[INFO] No command line input provided. Set \$git_user_id to $git_user_id"
fi

if ["$git_repo_id" = ""]; then
	git_repo_id = ""
	echo "[INFO] No command line input provided. Set \$git_repo_id to $git_repo_id"
fi

if ["$release_note" = ""]; then
	release_note = ""
	echo "[INFO] No command line input provided. Set \$release_note to $release_note"
fi

git init

git add .

git commit -m "$release_note"

git_remote = `git remote`

if ["$git_remote" = ""]; then
	if["$GIT_TOKEN" = ""]; then
		echo "[INFO] \$GIT_TOKEN (environment variable) is not set. Using the git credential in your environment."
		git remote add origin https://github.com/${git_user_id}/${git_repo_id}.git
	else
		git remote add origin https://${git_user_id}:${GIT_TOKEN}@github.com/${git_user_id}/${git_repo_id}.git
	fi
fi

git pull origin master

echo "Git pushing to https://github.com/${git_user_id}/${git_repo_id}.git"
git push origin master 2>&1 | grep -v 'To https'